# ChallengeBack - Docker Setup

## Como usar

### 1. Iniciar tudo (PostgreSQL + Migração + API)
```bash
docker-compose up -d
```

**O que acontece automaticamente:**
1. PostgreSQL inicia e fica pronto
2. Migração executa automaticamente
3. API inicia após a migração

### 2. Acessar a API
- **URL**: http://localhost:8080
- **PostgreSQL**: localhost:5432

## Comandos úteis

```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs apenas da API
docker-compose logs -f api

# Ver logs da migração
docker-compose logs migration

# Parar serviços
docker-compose down

# Rebuildar e iniciar
docker-compose up --build

# Limpar tudo
docker-compose down -v
```

## Configuração

- **PostgreSQL**: porta 5432, database `ChallengeBackDb`
- **API**: porta 8080
- **Dados**: persistidos no volume `postgres_data`
- **Migração**: executa automaticamente na inicialização
