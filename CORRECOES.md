# Documentação de Correções Aplicadas — CatálogoPOP

Este documento detalha todas as correções e melhorias de segurança, arquitetura, integração e manutenibilidade implementadas para elevar a conformidade do projeto **CatálogoPOP** aos padrões de produção.

---

## 🛠️ Detalhamento das Alterações

### 1. 🔒 Segurança e Ocultação de Segredos (Secrets Management)

* **Problema Identificado**: Senhas do banco de dados e segredos da chave JWT estavam expostos diretamente em arquivos versionados no repositório Git (`appsettings.json` e `ApplicationDbContextFactory.cs`).
* **Soluções Aplicadas**:
  * **Configuração de User Secrets**: Inicializamos o gerenciador de segredos do .NET no projeto [CatalogoPOP.API]. As credenciais reais foram salvas fora do repositório localmente.
  * **Limpeza de Arquivos de Configuração**: O arquivo [appsettings.json] foi limpo de chaves sensíveis e substituído por segredos dummy/placeholders genéricos.
  * **Carregamento Dinâmico**: Refatoramos o arquivo [ApplicationDbContextFactory.cs] da infraestrutura para que carregue dinamicamente as strings de conexão do banco a partir da pipeline do .NET (lendo *User Secrets*, arquivos de configurações de ambiente e variáveis de sistema) em vez de manter uma string estática fixada.

---

### 2. 🔑 Autenticação JWT, Proteção de Endpoints e Restrição de CORS

* **Problema Identificado**: Os endpoints do backend de POPs estavam abertos a chamadas não autorizadas, a política de CORS aceitava qualquer origem (*AllowAnyOrigin*) e o frontend não possuía login real, rotas protegidas ou envio do token Bearer.
* **Soluções Aplicadas**:
  * **Endpoint de Login**: Adicionamos o endpoint seguro `/api/auth/login` no backend ([Program.cs]) para receber credenciais e retornar um token JWT contendo assinatura de segurança e tempo de expiração definidos.
  * **Proteção de Rotas (Backend)**: Aplicamos o middleware `.RequireAuthorization()` em todo o grupo de endpoints `/api/pops` para assegurar que nenhuma requisição não autenticada consiga listar, criar, editar ou excluir dados.
  * **Restrição de CORS**: Redefinimos a política de CORS de `AllowAnyOrigin` para aceitar unicamente chamadas da porta padrão do frontend do Vite (`http://localhost:5173`).
  * **Tela de Login com Design Premium**: Criamos a página de login em [Login.jsx] contendo *glassmorphism*, fundos animados e layout premium responsivo.
  * **Proteção de Rotas (Frontend)**: Criamos o wrapper `ProtectedRoute` em [App.jsx] que redireciona automaticamente o usuário para a tela de login se ele não possuir um token ativo.
  * **Envio Automatizado de Token (Interceptor)**: Atualizamos o arquivo de serviço [api.js] com um interceptor Axios que anexa automaticamente o token salvo no `localStorage` como `Authorization: Bearer <token>` em todas as chamadas HTTP enviadas ao backend.

---

### 3. 🎨 Modularização de Estilos (CSS Modules)

* **Problema Identificado**: O uso de estilos internos com blocos `<style dangerouslySetInnerHTML>` poluía o código JavaScript/JSX e prejudicava o reuso e a manutenção das interfaces de layout.
* **Soluções Aplicadas**:
  * **Extração de Estilos**: Extraímos os estilos embutidos do layout nos seguintes novos arquivos CSS Modules:
    * [Layout.module.css]
    * [Navbar.module.css]
    * [Sidebar.module.css]
  * **Refatoração dos Componentes**: Atualizamos as referências de estilo dentro de [Layout.jsx], [Navbar.jsx] e [Sidebar.jsx] para utilizar a propriedade importada de classes CSS (`styles.<nomeDaClasse>`), eliminando completamente as injeções diretas em tags HTML de estilo.

---

## 🚀 Como testar localmente

1. **Credenciais para Login**:
   * **Usuário**: `admin`
   * **Senha**: `admin123`

2. **Backend**:
   * Certifique-se de que os User Secrets locais estão definidos caso queira alterar a senha ou string padrão:
     ```bash
     dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=catalogopop_db;Username=postgres;Password=<sua_senha>"
     dotnet user-secrets set "JwtSettings:Secret" "<sua_chave_secreta_com_no_minimo_32_caracteres>"
     ```
   * Execute o backend na pasta `backend/`:
     ```bash
     dotnet run --project CatalogoPOP.API
     ```

3. **Frontend**:
   * Execute o frontend na pasta `frontend/`:
     ```bash
     npm run dev
     ```
