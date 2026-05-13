<div align="center">

<img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10"/>
<img src="https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL"/>
<img src="https://img.shields.io/badge/Clean%20Architecture-✓-22C55E?style=for-the-badge" alt="Clean Architecture"/>
<img src="https://img.shields.io/badge/CQRS-✓-F59E0B?style=for-the-badge" alt="CQRS"/>
<img src="https://img.shields.io/badge/Testes-22%2F22%20✓-22C55E?style=for-the-badge" alt="Tests"/>

<br/><br/>

# 📋 HGTX™ CatálogoPOP
### *Sistema de Gestão de Procedimentos Operacionais Padrão*

> **Uma API RESTful robusta construída com as melhores práticas do ecossistema .NET moderno.**  
> Clean Architecture · CQRS com MediatR · Minimal APIs · JWT · Rate Limiting · PostgreSQL

<br/>

[🚀 Início Rápido](#-início-rápido) · [🏗️ Arquitetura](#️-arquitetura) · [📡 Endpoints](#-endpoints-da-api) · [🧪 Testes](#-testes-unitários) · [📦 Tecnologias](#-tecnologias-utilizadas)

</div>

---

## 📖 Sobre o Projeto

O **HGTX™ CatálogoPOP** é uma API de gerenciamento de **Procedimentos Operacionais Padrão (POP)** — documentos que descrevem passo a passo como executar processos dentro de uma organização (ex: higienização de equipamentos, atendimento ao cliente, etc.).

O projeto foi desenvolvido como um **desafio técnico backend**, aplicando padrões arquiteturais avançados do mundo corporativo:

| 🎯 Objetivo | ✅ Resultado |
|---|---|
| Arquitetura limpa e escalável | Clean Architecture com 4 camadas |
| Código organizado e manutenível | CQRS com Commands e Queries separados |
| Segurança no acesso | Autenticação JWT Bearer |
| Proteção contra abusos | Rate Limiting (10 req/10s) |
| Banco de dados relacional | PostgreSQL com EF Core e Migrations |
| Qualidade garantida | 22 testes unitários (100% aprovados) |
| Documentação interativa | Scalar UI integrada |

---

## 🏗️ Arquitetura

O projeto utiliza **Clean Architecture** (Arquitetura Limpa), que organiza o código em camadas com dependências sempre apontando para dentro — do mais externo para o mais interno.

```
┌─────────────────────────────────────────────────────────┐
│                    📡 CatalogoPOP.API                   │
│         Minimal APIs · JWT · Rate Limiting · Scalar     │
├─────────────────────────────────────────────────────────┤
│                 📐 CatalogoPOP.Application              │
│         CQRS · MediatR · AutoMapper · Validators        │
├─────────────────────────────────────────────────────────┤
│               🔌 CatalogoPOP.Infrastructure             │
│         EF Core · PostgreSQL · Repositories · Migrations│
├─────────────────────────────────────────────────────────┤
│                  💎 CatalogoPOP.Domain                  │
│         Entities · Enums · Interfaces · Exceptions      │
└─────────────────────────────────────────────────────────┘
```

### Por que Clean Architecture?

- **Domínio isolado**: As regras de negócio não dependem de banco de dados, frameworks ou interfaces.
- **Testabilidade**: É fácil testar cada camada de forma isolada.
- **Substituibilidade**: Podemos trocar o PostgreSQL por SQL Server, ou o EF Core por Dapper, sem afetar as regras de negócio.
- **Legibilidade**: Qualquer desenvolvedor encontra o arquivo que procura rapidamente.

### Estrutura de Arquivos

```
projeto-crud-catalogopop/
│
├── 📂 backend/                          # Pasta do Servidor (.NET 10)
│   ├── CatalogoPOP.slnx                 # Arquivo da Solução
│   │
│   ├── 💎 CatalogoPOP.Domain/
│   │   ├── Entities/
│   │   │   └── ProcedimentoOperacional.cs   # Entidade principal
│   │   └── ...
│   │
│   ├── 📐 CatalogoPOP.Application/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   └── ...
│   │
│   ├── 🔌 CatalogoPOP.Infrastructure/
│   │   ├── Persistence/
│   │   ├── Migrations/
│   │   └── ...
│   │
│   ├── 📡 CatalogoPOP.API/
│   │   ├── Program.cs                       # Endpoints
│   │   └── appsettings.json                 # Configurações
│   │
│   └── 🧪 CatalogoPOP.Tests/
│       └── Domain/
│           └── ProcedimentoOperacionalTests.cs
│
└── 📂 frontend/                         # Pasta da Interface (em breve)
```

---

## 💎 Camada de Domínio

O coração do sistema. Contém as **regras de negócio** que existem independentemente de qualquer tecnologia.

### Entidade: `ProcedimentoOperacional`

A entidade principal do sistema. Usa `private set` para garantir encapsulamento — os dados só podem ser alterados por métodos internos da própria classe.

```csharp
public class ProcedimentoOperacional
{
    public Guid Id { get; private set; }
    public string Codigo { get; private set; }       // Ex: "HIG-001"
    public string Titulo { get; private set; }
    public string Versao { get; private set; }       // Ex: "1.0", "1.1"
    public StatusProcedimento Status { get; private set; }
    public bool IsDeleted { get; private set; }      // Soft Delete
    // ...
}
```

### ✨ Regras de Negócio Implementadas

| Regra | Como funciona |
|---|---|
| **Versionamento Automático** | Ao ser aprovado pela 1ª vez, vai de `v1.0` → `v1.1`, depois `v1.2`, e assim por diante |
| **Status Inicial** | Todo POP começa com status `Rascunho` |
| **Soft Delete** | `Excluir()` marca `IsDeleted = true` em vez de apagar do banco |
| **Proteção contra dupla exclusão** | Lança `DomainException` ao tentar excluir o que já está excluído |
| **Validação de campos** | `Codigo`, `Titulo`, `Descricao` e `Responsavel` não podem ser vazios |

---

## 📐 Camada de Aplicação (CQRS)

Implementa o padrão **CQRS (Command Query Responsibility Segregation)** — separa as operações de **escrita** (Commands) das de **leitura** (Queries).

```
📥 COMMAND (Escrita)                   📤 QUERY (Leitura)
─────────────────────                  ────────────────────
CriarProcedimentoCommand               ObterTodosProcedimentosQuery
        │                                       │
        ▼                                       ▼
CriarProcedimentoCommandHandler         (Handler de leitura)
        │
        ├─ Valida dados (FluentValidation)
        ├─ Verifica duplicidade de código
        ├─ Cria a entidade
        └─ Salva no banco via Repositório
```

### MediatR — O Mediador

O **MediatR** funciona como um "roteador interno": quando um endpoint envia um Command, o MediatR automaticamente encontra e executa o Handler correto. Isso elimina dependências diretas entre camadas.

```csharp
// No endpoint:
var result = await mediator.Send(new CriarProcedimentoCommand(...));

// O MediatR encontra automaticamente o CriarProcedimentoCommandHandler e o executa.
```

---

## 🔌 Camada de Infraestrutura

Responsável pela comunicação com o banco de dados. Implementa os contratos (interfaces) definidos no Domínio.

### Configuração com Fluent API

```csharp
// No DbContext, configuramos o mapeamento da entidade para o banco:
entity.HasKey(p => p.Id);
entity.HasIndex(p => p.Codigo).IsUnique();  // Garante que o código é único no banco
entity.Property(p => p.Titulo).HasMaxLength(150).IsRequired();
entity.HasQueryFilter(p => !p.IsDeleted);   // Soft Delete automático em todas as queries
```

### Regra Anti-Duplicidade no Repositório

```csharp
public async Task<bool> CodigoExisteAsync(string codigo)
    => await _context.ProcedimentosOperacionais
        .AnyAsync(p => p.Codigo == codigo);
```

---

## 📡 Endpoints da API

A API usa **Minimal APIs** do .NET — uma forma mais leve e rápida de definir endpoints do que os Controllers tradicionais.

### Base URL: `http://localhost:5200`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `POST` | `/api/pops` | Cria um novo POP | 🔓 Público* |
| `GET` | `/api/pops` | Lista todos os POPs | 🔓 Público* |

> *A autenticação JWT está configurada e pode ser ativada adicionando `.RequireAuthorization()` nos endpoints.

### 📨 Criar um POP — `POST /api/pops`

**Request Body:**
```json
{
  "titulo": "Higienização de Equipamentos",
  "codigo": "HIG-001",
  "descricao": "Procedimento padrão para higienização de todos os equipamentos da linha de produção.",
  "departamento": 1,
  "responsavel": "João Silva"
}
```

**Response `201 Created`:**
```json
{
  "id": "a3f8c2b1-...",
  "codigo": "HIG-001",
  "titulo": "Higienização de Equipamentos",
  "versao": "1.0",
  "status": "Rascunho",
  "departamento": "Producao",
  "responsavel": "João Silva",
  "dataRevisao": null
}
```

**Response `400 Bad Request` (duplicidade):**
```json
{
  "error": "Já existe um Procedimento com o código 'HIG-001'."
}
```

### 📤 Listar todos — `GET /api/pops`

**Response `200 OK`:**
```json
[
  {
    "id": "a3f8c2b1-...",
    "codigo": "HIG-001",
    "titulo": "Higienização de Equipamentos",
    "versao": "1.0",
    "status": "Rascunho"
  }
]
```

---

## 🔐 Segurança

### JWT (JSON Web Token)

A API está configurada para autenticação via **JWT Bearer Token**. Para usar:

1. Gere um token com as credenciais corretas (endpoint de login a implementar).
2. Inclua o header: `Authorization: Bearer <seu-token>`

**Configuração em `appsettings.json`:**
```json
"JwtSettings": {
  "Secret": "sua-chave-secreta-com-256-bits-minimo",
  "Issuer": "CatalogoPOP.API",
  "Audience": "CatalogoPOP.Client",
  "ExpiresInHours": 8
}
```

### Rate Limiting

Proteção contra abuso e ataques DDoS com **Fixed Window Rate Limiter**:

```
Limite: 10 requisições a cada 10 segundos por cliente.
Fila: Até 2 requisições aguardam na fila antes de serem rejeitadas.
```

---

## 🧪 Testes Unitários

O projeto conta com **22 testes unitários** (100% aprovados) usando **xUnit** + **FluentAssertions**.

```
Execução de Teste Bem-sucedida.
Total de testes: 22
     Aprovados: 22
Tempo total: 5,26 Segundos
```

### Grupos de Testes

| Grupo | Quantidade | O que testa |
|---|---|---|
| **Criação válida** | 4 | Status inicial, versão 1.0, ID gerado, IsDeleted false |
| **Validações de entrada** | 12 | Campos obrigatórios (string vazia, espaços e null) |
| **Versionamento** | 4 | Incremento na aprovação, múltiplas aprovações, sem incremento indevido |
| **Soft Delete** | 2 | Marcar como excluído, erro ao excluir duas vezes |

### Como Executar os Testes

```bash
dotnet test CatalogoPOP.Tests/CatalogoPOP.Tests.csproj --verbosity normal
```

---

## 📦 Tecnologias Utilizadas

### Backend & Framework
| Tecnologia | Versão | Para que serve |
|---|---|---|
| **.NET** | `10.0` | Plataforma principal |
| **C#** | `13` | Linguagem de programação |
| **ASP.NET Core Minimal APIs** | `10.0` | Definição dos endpoints HTTP |

### Padrões e Bibliotecas
| Biblioteca | Versão | Para que serve |
|---|---|---|
| **MediatR** | `12.0` | Implementação do padrão CQRS/Mediator |
| **AutoMapper** | `12.0` | Mapeamento entre Entidades e DTOs |
| **FluentValidation** | `11.7` | Validação declarativa e legível de dados |
| **Scrutor** | `7.0` | Auto-registro de serviços por convenção |

### Banco de Dados
| Biblioteca | Versão | Para que serve |
|---|---|---|
| **Entity Framework Core** | `10.0.4` | ORM (Object-Relational Mapper) |
| **Npgsql EF Core** | `10.0.4` | Driver do PostgreSQL para EF Core |
| **PostgreSQL** | `16+` | Banco de dados relacional |

### Segurança & Documentação
| Biblioteca | Versão | Para que serve |
|---|---|---|
| **JwtBearer** | `10.0.4` | Autenticação via token JWT |
| **AspNetCore.OpenApi** | `10.0.4` | Geração automática de spec OpenAPI |
| **Scalar.AspNetCore** | `2.0.14` | Interface visual para testar a API |

### Testes
| Biblioteca | Versão | Para que serve |
|---|---|---|
| **xUnit** | `2.9` | Framework de testes |
| **FluentAssertions** | `6.12` | Assertions legíveis e expressivas |
| **coverlet** | `6.0` | Cobertura de código |

---

## 🚀 Início Rápido

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)

### 1. Clonar o repositório

```bash
git clone https://github.com/MatheusAlves08/projeto-crud-catalogopop.git
cd projeto-crud-catalogopop
```

### 2. Configurar o banco de dados

Edite o arquivo `backend/CatalogoPOP.API/appsettings.json` com suas credenciais do PostgreSQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=catalogopop_db;Username=postgres;Password=SUA_SENHA"
}
```

### 3. Aplicar as Migrations (criar as tabelas)

```bash
dotnet ef database update --startup-project backend/CatalogoPOP.API/CatalogoPOP.API.csproj
```

### 4. Executar a API

```bash
dotnet run --project backend/CatalogoPOP.API/CatalogoPOP.API.csproj
```

A API estará disponível em `http://localhost:5148`.

### 5. Acessar a documentação interativa

Abra no navegador: **[http://localhost:5148/scalar/v1](http://localhost:5148/scalar/v1)**

> O Scalar é uma interface visual moderna (similar ao Postman) onde você pode explorar e testar todos os endpoints diretamente no navegador.

### 6. Executar os testes

```bash
dotnet test backend/CatalogoPOP.Tests/CatalogoPOP.Tests.csproj
```

---

## 🗺️ Roadmap

Funcionalidades previstas para as próximas versões:

- [ ] 🔐 **Endpoint de Login** — emitir tokens JWT com credenciais
- [ ] ✏️ **Atualizar POP** — `PUT /api/pops/{id}` com versionamento automático
- [ ] 🗑️ **Excluir POP** — `DELETE /api/pops/{id}` (Soft Delete)
- [ ] 🔍 **Buscar por ID** — `GET /api/pops/{id}`
- [ ] 🔎 **Filtros e paginação** — filtrar por Status, Departamento, Código
- [ ] 🐳 **Docker Compose** — containerização completa da API + banco

---

## 📚 Conceitos Aprendidos

Este projeto foi desenvolvido como um estudo prático dos seguintes padrões e conceitos:

> **Clean Architecture** — Separação de responsabilidades em camadas com dependências controladas.

> **CQRS** — Separar operações de leitura (Query) das de escrita (Command) para melhor escalabilidade e organização.

> **Domain-Driven Design (DDD)** — Modelar o código refletindo o negócio real, com entidades ricas em comportamento.

> **Repository Pattern** — Abstrair o acesso ao banco de dados atrás de uma interface, desacoplando a lógica de negócio da persistência.

> **Soft Delete** — Nunca apagar dados do banco; marcar registros como "excluídos" para preservar histórico.

> **Testes Unitários** — Validar o comportamento de cada unidade de código de forma isolada e automática.

---

<div align="center">

Desenvolvido por **[Matheus Alves](https://github.com/MatheusAlves08)** 

<br/>

<img src="https://img.shields.io/badge/Feito%20com-C%23%20%E2%9D%A4%EF%B8%8F-512BD4?style=flat-square&logo=csharp"/>
<img src="https://img.shields.io/badge/Arquitetura-Clean%20Architecture-22C55E?style=flat-square"/>
<img src="https://img.shields.io/badge/Status-Em%20Desenvolvimento-F59E0B?style=flat-square"/>

</div>
