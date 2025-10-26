# 🧩 FullStack Lead Management System

Aplicação desenvolvida como desafio técnico de estágio — arquitetura completa SPA React + .NET 6 Web API + SQL Server, com integração via Axios + React Query, suporte a Swagger, Docker Compose e testes unitários e de integração com xUnit, Moq e EFCore.InMemory.

## Visão geral
Este repositório contém uma aplicação full-stack de exemplo para gerenciamento de leads:
- Backend: Web API em .NET 6 usando Entity Framework Core, com organização em Commands/Queries e Handlers.
- Frontend: SPA em React + Vite + TypeScript consumindo a API.
- Testes: testes unitários e de integração com xUnit (projeto `api.Tests`).

O objetivo é demonstrar uma arquitetura simples, integração entre frontend e backend, uso de EF Core (migrations) e cobertura de testes.

## Arquitetura e estrutura de pastas
Estrutura principal (resumida):
- `api/` — projeto backend (.NET)
	- `Controllers/LeadsController.cs`
	- `Application/Commands`, `Queries`, `Handlers`, `DTOs`
	- `Domain/Entities`, `Enums`, `Services`
	- `Data/LeadDbContext.cs`
	- `Migrations/`
	- `Program.cs`
- `api.Tests/` — testes (xUnit)
- `frontend/` — app React + Vite
	- `src/api` — cliente HTTP e wrappers
	- `src/ui` — páginas e componentes

Padrão: Web API com variação de CQRS (Commands/Queries). DbContext centraliza acesso ao banco. Frontend separado e consumindo via HTTP.

## Tecnologias
- Backend: .NET 6, Entity Framework Core, xUnit
- Frontend: React, TypeScript, Vite
- Banco (dev): SQL Server (ou outro provider EF Core)

## Pré-requisitos
- .NET 6 SDK (ou compatível)
- Node.js 16+ (recomendado)
- npm ou yarn
- SQL Server local ou outro DB suportado pelo EF Core

## Variáveis de ambiente importantes
- `ASPNETCORE_ENVIRONMENT` — ambiente (Development/Production)
- `ConnectionStrings__DefaultConnection` — string de conexão do banco (conforme `appsettings.json`)

Exemplo (Windows, bash):
```bash
export ConnectionStrings__DefaultConnection="Server=(localdb)\\mssqllocaldb;Database=LeadDb;Trusted_Connection=True;MultipleActiveResultSets=true"
export ASPNETCORE_ENVIRONMENT=Development
```

## Rodando localmente
Os exemplos abaixo assumem que você está na raiz do repositório (`f:/DTI/projetoFullStack`) ou navegue para os diretórios indicados. O shell padrão é `bash.exe` — os comandos abaixo usam sintaxe bash.

### 1) Backend (.NET)
```bash
cd api
dotnet restore

# (Opcional) aplicar migrations e criar/atualizar o banco
dotnet ef database update

# executar a API em modo desenvolvimento
dotnet run
```

Quando iniciado, a saída mostrará a URL/porta (p.ex. `http://localhost:5000` ou HTTPS). Verifique `Properties/launchSettings.json` ou `appsettings.Development.json` para portas específicas.

### 2) Frontend (React + Vite)
```bash
cd frontend
npm install
npm run dev
```
O Vite exibirá a URL do servidor de desenvolvimento (p.ex. `http://localhost:5173`).

### 3) Rodando ambos juntos
- Inicie primeiro a API (`dotnet run` em `api/`).
- Em seguida, inicie o frontend (`npm run dev` em `frontend/`).
- Ajuste a base URL do cliente no frontend (`frontend/src/api/client.ts`) para apontar à URL da API, se necessário.

## Banco de dados e migrations
As migrations do EF Core estão em `api/Migrations/`.

- Criar migration:
```bash
cd api
dotnet ef migrations add NomeDaMigration
```
- Aplicar migrations:
```bash
dotnet ef database update
```

Obs.: A migration inicial já existe (`20251025030448_Init.cs`).

## Endpoints da API (principais)
Abra `api/Controllers/LeadsController.cs` para detalhes. Abaixo estão os endpoints principais presumidos:

1) GET /api/leads
- Descrição: lista leads.
- Resposta (200): array de objetos (veja DTOs em `api/Application/DTOs`).

2) POST /api/leads/{id}/accept
- Descrição: aceita um lead (altera status para Accepted e possivelmente dispara e-mail).

3) POST /api/leads/{id}/decline
- Descrição: recusa um lead (altera status para Declined).

Exemplos curl (substitua a porta e o id conforme seu ambiente):
```bash
curl http://localhost:5000/api/leads

curl -X POST http://localhost:5000/api/leads/00000000-0000-0000-0000-000000000000/accept

curl -X POST http://localhost:5000/api/leads/00000000-0000-0000-0000-000000000000/decline
```

## Exemplos de request/response
GET lista de leads:
```bash
curl http://localhost:5000/api/leads
```
Resposta (exemplo):
```json
[
	{
		"id": "b0a8a9b4-xxxx-xxxx-xxxx-xxxxxxxx",
		"name": "João Silva",
		"email": "joao@example.com",
		"status": "Invited"
	}
]
```

Aceitar lead:
```bash
curl -X POST http://localhost:5000/api/leads/b0a8a9b4-xxxx-xxxx-xxxx-xxxxxxxx/accept
```

## Testes
O projeto de testes está em `api.Tests/`. Para rodar os testes:
```bash
cd api.Tests
dotnet test
```

Os testes usam xUnit; se o projeto está usando Moq ou EFCore.InMemory, a suíte deve rodar sem um DB real.

## Serviço de e-mail fake
Há um serviço fake em `api/Domain/Services/FakeEmailService.cs` que simula envio de e-mails para desenvolvimento. Para produção, implemente `IEmailService` real e registre no DI.

## SQL Server LocalDB (Windows)
Para desenvolvimento local no Windows, você pode usar o SQL Server LocalDB, que é leve e fácil de configurar. A string de conexão típica para LocalDB é:

```bash
Server=(localdb)\\mssqllocaldb;Database=LeadDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

## Docker (exemplo sugerido)
O repositório pode ser dockerizado; não há `Dockerfile`/`docker-compose.yml` por padrão, mas uma configuração sugerida:
- Serviço `db` (SQL Server ou Postgres)
- Serviço `api` (build de `./api`)
- Serviço `frontend` (build de `./frontend`)

Se quiser, posso criar exemplos de `Dockerfile` e `docker-compose.yml` adaptados ao projeto.

## Debugging e troubleshooting
- CORS: se o frontend não conseguir acessar a API, habilite CORS em `Program.cs` (AddCors / UseCors).
- Ports: verifique `Properties/launchSettings.json` e `appsettings.*.json`.
- EF: execute `dotnet ef database update` e verifique a connection string.
- Tests: rode `dotnet test --filter "FullyQualifiedName~NomeDoTeste"` para isolar um teste.
- No Windows + bash, cuide do escaping em connection strings (duas barras invertidas `\\`).

Debug no VS Code/Visual Studio:
- Abra a solução `api.sln` e use o debugger. Coloque breakpoints em `AcceptLeadHandler`, `DeclineLeadHandler` ou `LeadsController`.


#### Feito com ❤️ por Gabriel Amâncio de Oliveira