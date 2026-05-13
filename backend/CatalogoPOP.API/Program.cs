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
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

// O WebApplicationBuilder é o responsável por configurar os serviços da nossa aplicação.
var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DOS SERVIÇOS (CONTAINER DE INJEÇÃO DE DEPENDÊNCIA) ---

// 1. Registra as outras camadas do projeto. 
builder.Services.AddInfrastructure(builder.Configuration); // Registra Banco de Dados e Repositórios
builder.Services.AddApplication();                        // Registra MediatR, Mappers e Validadores

// 2. Configura o OpenAPI (.NET 10 Built-in).
// No .NET 10, o suporte a OpenAPI já vem integrado, não precisamos obrigatoriamente de pacotes externos como Swagger.
builder.Services.AddOpenApi();

// 3. Configura Autenticação JWT (Segurança).
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

// 4. Configura Rate Limiting.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// 6. Configura CORS para permitir chamadas do Frontend.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses()
    .AsImplementedInterfaces()
    .WithScopedLifetime());

var app = builder.Build();

// --- CONFIGURAÇÃO DO PIPELINE HTTP (MIDDLEWARES) ---

// 1. Usa a política de CORS definida acima.
app.UseCors("AllowFrontend");

// Middleware de tratamento global de erros.
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (DomainException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        // Log do erro real no console para o desenvolvedor ver durante o dotnet run
        Console.WriteLine($"[ERRO INTERNO]: {ex.Message}");
        Console.WriteLine(ex.StackTrace);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro inesperado. Verifique o console do servidor." });
    }
});

// No .NET 10, mapeamos o endpoint do OpenAPI (gera o JSON da documentação)
// E também o Scalar, que é uma interface visual LINDA para testar a API.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Acessível em: http://localhost:PORTA/scalar/v1
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

// --- ENDPOINTS (MINIMAL APIs) ---

var apiGroup = app.MapGroup("/api/pops")
    .WithTags("Procedimentos Operacionais Padrão (POP)")
    .RequireRateLimiting("fixed");

apiGroup.MapPost("/", async (CriarProcedimentoCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/pops/{result.Id}", result);
})
.WithName("CriarProcedimento");

apiGroup.MapGet("/", async (IMediator mediator) =>
{
    var result = await mediator.Send(new ObterTodosProcedimentosQuery());
    return Results.Ok(result);
})
.WithName("ObterTodos");

app.Run();
