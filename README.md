# ChallengeBack - API Backend

## 📋 Sobre o Projeto

O ChallengeBack é uma API REST desenvolvida em .NET 9 que gerencia empresas, fornecedores e suas relações. O projeto implementa uma arquitetura limpa (Clean Architecture) com separação clara de responsabilidades entre as camadas.

## 🛠️ Tecnologias Utilizadas

### Core Framework
- **.NET 9.0** - Framework principal
- **ASP.NET Core** - Para construção da API REST
- **Entity Framework Core 9.0.1** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados relacional

### Bibliotecas e Pacotes
- **AutoMapper 13.0.1** - Mapeamento entre objetos
- **Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4** - Provider PostgreSQL para EF Core
- **Microsoft.AspNetCore.OpenApi 9.0.9** - Documentação automática da API

### Testes
- **xUnit 2.9.0** - Framework de testes
- **Moq 4.20.72** - Mocking para testes unitários
- **Testcontainers 4.7.0** - Containers para testes de integração
- **Testcontainers.PostgreSql 4.7.0** - Container PostgreSQL para testes

### DevOps
- **Docker** - Containerização da aplicação
- **Docker Compose** - Orquestração de serviços

## 🏗️ Arquitetura do Projeto

O projeto segue os princípios da **Clean Architecture** com as seguintes camadas:

```
src/
├── ChallengeBack.Domain/          # Camada de Domínio
│   ├── Entities/                  # Entidades do domínio
│   └── Enums/                     # Enumerações
├── ChallengeBack.Application/     # Camada de Aplicação
│   ├── Dto/                      # Data Transfer Objects
│   ├── Interfaces/               # Contratos e interfaces
│   ├── Mappers/                  # Configuração do AutoMapper
│   └── Services/                  # Serviços de aplicação
├── ChallengeBack.Infrastructure/ # Camada de Infraestrutura
│   ├── Data/                     # Contexto do banco de dados
│   ├── Migrations/               # Migrações do EF Core
│   ├── Repositories/             # Implementação dos repositórios
│   └── CepApi/                   # Integração com API de CEP
├── ChallengeBack.Api/            # Camada de Apresentação
│   ├── Controllers/              # Controllers da API
│   ├── Extensions/               # Extensões e configurações
│   └── Middleware/               # Middlewares customizados
└── ChallengeBack.Tests/          # Camada de Testes
    ├── Repositories/             # Testes de repositórios
    ├── Services/                 # Testes de serviços
    └── Support/                  # Utilitários de teste
```

## 📊 Entidades do Domínio

### Company (Empresa)
- **Id**: Identificador único
- **Cnpj**: CNPJ da empresa
- **FantasyName**: Nome fantasia
- **ZipCode**: CEP
- **State**: Estado
- **CreatedAt/UpdatedAt**: Timestamps de auditoria

### Supplier (Fornecedor)
- **Id**: Identificador único
- **Type**: Tipo (Individual ou Company)
- **Cpf**: CPF (apenas para pessoa física)
- **Cnpj**: CNPJ (apenas para pessoa jurídica)
- **Name**: Nome/Razão social
- **Email**: E-mail
- **ZipCode**: CEP
- **Rg**: RG (apenas para pessoa física)
- **BirthDate**: Data de nascimento (apenas para pessoa física)
- **CreatedAt/UpdatedAt**: Timestamps de auditoria

### CompanySupplier (Relação Empresa-Fornecedor)
- **Id**: Identificador único
- **CompanyId**: ID da empresa
- **SupplierId**: ID do fornecedor
- **Company/Supplier**: Propriedades de navegação

## 🚀 Endpoints da API

### Companies (`/api/v1/company`)
- `POST /` - Criar empresa
- `GET /` - Listar empresas (com paginação e filtros)
- `PUT /{id}` - Atualizar empresa
- `DELETE /{id}` - Deletar empresa

### Suppliers (`/api/v1/supplier`)
- `POST /` - Criar fornecedor
- `GET /` - Listar fornecedores (com paginação e filtros)
- `PUT /{id}` - Atualizar fornecedor
- `DELETE /{id}` - Deletar fornecedor

### Company-Supplier Relations (`/api/v1/company-supplier`)
- `POST /` - Associar empresa e fornecedor
- `GET /` - Listar associações (com paginação)
- `DELETE /{companyId}/{supplierId}` - Remover associação

## 🐳 Como Executar o Projeto

### Pré-requisitos
- Docker e Docker Compose instalados
- .NET 9 SDK (para desenvolvimento local)

### Opção 1: Docker Compose (Recomendado)

```bash
# Clone o repositório
git clone <repository-url>
cd ChallengeBack

# Inicie todos os serviços (PostgreSQL + API)
docker-compose up -d

# A API estará disponível em: http://localhost:8080
# PostgreSQL estará disponível em: localhost:5432
```

### Opção 2: Desenvolvimento Local

```bash
# 1. Inicie o PostgreSQL via Docker
docker-compose up postgres -d

# 2. Navegue até a pasta da API
cd src/ChallengeBack.Api

# 3. Restaure as dependências
dotnet restore

# 4. Execute a API
dotnet run
```

### Comandos Úteis do Docker

```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs apenas da API
docker-compose logs -f api

# Parar serviços
docker-compose down

# Rebuildar e iniciar
docker-compose up --build

# Limpar volumes (dados do banco)
docker-compose down -v
```

## 🧪 Executando Testes

```bash
# Navegue até a pasta de testes
cd src/ChallengeBack.Tests

# Execute todos os testes
dotnet test

```

## 📝 Configurações

### Banco de Dados
- **Host**: localhost (desenvolvimento) / postgres (Docker)
- **Porta**: 5432
- **Database**: ChallengeBackDb
- **Usuário**: postgres
- **Senha**: postgres

### API
- **Porta**: 8080
- **Ambiente**: Development (local) / Docker (container)

## 🔧 Funcionalidades

- ✅ CRUD completo para Empresas
- ✅ CRUD completo para Fornecedores
- ✅ Gestão de relacionamentos Empresa-Fornecedor
- ✅ Validação de CPF/CNPJ baseada no tipo de pessoa
- ✅ Integração com API de CEP
- ✅ Paginação e filtros nas listagens
- ✅ Testes
- ✅ Documentação automática da API (OpenAPI + Swagger UI)

## 📚 Documentação da API

Quando a API estiver rodando, acesse:
- **Swagger UI**: http://localhost:{PORT}/swagger (interface interativa)
- **OpenAPI JSON**: http://localhost:{PORT}/swagger/v1/swagger.json

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request
