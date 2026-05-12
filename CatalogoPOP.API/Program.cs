using CatalogoPOP.Application;
using CatalogoPOP.Application.Commands;
using CatalogoPOP.Application.Queries;
using CatalogoPOP.Domain.Exceptions;
using CatalogoPOP.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

// O WebApplicationBuilder é o responsável por configurar os serviços da nossa aplicação.
// Pense nele como a fase de "preparação" onde dizemos ao .NET tudo o que vamos usar.
var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DOS SERVIÇOS (CONTAINER DE INJEÇÃO DE DEPENDÊNCIA) ---

// 1. Registra as outras camadas do projeto. 
// Chamamos os métodos de extensão que criamos nas outras bibliotecas para organizar o código.
builder.Services.AddInfrastructure(builder.Configuration); // Registra Banco de Dados e Repositórios
builder.Services.AddApplication();                        // Registra MediatR, Mappers e Validadores

// 2. Configura o OpenAPI/Swagger.
// O Swagger cria uma página web automática para testarmos nossa API sem precisar de ferramentas externas.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configura Autenticação JWT (Segurança).
// O JWT (JSON Web Token) é um padrão para enviar informações de identidade de forma segura.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Em desenvolvimento, permitimos HTTP simples
    options.SaveToken = true;            // Salva o token para podermos ler depois se necessário
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true // Garante que tokens expirados sejam rejeitados
    };
});

builder.Services.AddAuthorization(); // Habilita o sistema de permissões

// 4. Configura Rate Limiting (Proteção contra excesso de requisições).
// Isso impede que um usuário ou robô "trave" o sistema fazendo milhares de pedidos por segundo.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10; // Permite apenas 10 pedidos...
        opt.Window = TimeSpan.FromSeconds(10); // ...dentro de cada janela de 10 segundos.
        opt.QueueLimit = 2; // Se passar de 10, coloca até 2 pedidos na fila de espera.
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// 5. Scrutor (Opcional: Registro Automático).
// O Scrutor varre o projeto em busca de classes e interfaces e as registra no .NET sozinho.
// Isso evita ter que escrever 'services.AddScoped' para cada nova classe que criarmos.
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses()
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// builder.Build() finaliza as configurações e cria a "Aplicação" propriamente dita.
var app = builder.Build();

// --- CONFIGURAÇÃO DO PIPELINE HTTP (MIDDLEWARES) ---
// O Pipeline é o caminho que cada requisição percorre até chegar no banco e voltar.

// Middleware de tratamento global de erros.
// Usamos este bloco para "abraçar" toda a aplicação e capturar erros inesperados.
app.Use(async (context, next) =>
{
    try
    {
        await next(); // Tenta executar o próximo passo da API
    }
    catch (DomainException ex)
    {
        // Se cair aqui, é porque uma regra de negócio nossa (ex: duplicidade) foi violada.
        // Retornamos 400 (Bad Request) com a mensagem explicativa.
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception)
    {
        // Se cair aqui, foi um erro técnico não previsto.
        // Retornamos 500 (Erro de Servidor) para não expor detalhes técnicos ao usuário.
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro inesperado. Tente novamente mais tarde." });
    }
});

// Se estivermos em modo de desenvolvimento, mostra a página do Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redireciona chamadas HTTP para HTTPS (seguro)

app.UseAuthentication();   // Verifica QUEM é o usuário (Token)
app.UseAuthorization();    // Verifica O QUE o usuário pode fazer

app.UseRateLimiter();      // Aplica as regras de limite de velocidade definidas acima

// --- ENDPOINTS (MINIMAL APIs) ---
// Aqui definimos os endereços (URLs) da nossa API.

// Criamos um grupo para não ter que repetir "/api/pops" em cada endpoint.
var apiGroup = app.MapGroup("/api/pops")
    .WithTags("Procedimentos Operacionais Padrão (POP)") // Organiza no Swagger
    .RequireRateLimiting("fixed");                       // Protege este grupo com o Rate Limit

// Endpoint: Criar um novo POP.
// Recebe os dados via JSON, valida e envia para o MediatR processar.
apiGroup.MapPost("/", async (CriarProcedimentoCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/pops/{result.Id}", result); // Retorna 201 Created
})
.WithName("CriarProcedimento")
.WithOpenApi();

// Endpoint: Listar todos os POPs.
// Simplesmente pede ao MediatR a lista de todos os registros.
apiGroup.MapGet("/", async (IMediator mediator) =>
{
    var result = await mediator.Send(new ObterTodosProcedimentosQuery());
    return Results.Ok(result); // Retorna 200 OK com a lista
})
.WithName("ObterTodos")
.WithOpenApi();

// Inicia o servidor da API.
app.Run();
