# Stage de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/ChallengeBack.Api/ChallengeBack.Api.csproj", "src/ChallengeBack.Api/"]
COPY ["src/ChallengeBack.Application/ChallengeBack.Application.csproj", "src/ChallengeBack.Application/"]
COPY ["src/ChallengeBack.Domain/ChallengeBack.Domain.csproj", "src/ChallengeBack.Domain/"]
COPY ["src/ChallengeBack.Infrastructure/ChallengeBack.Infrastructure.csproj", "src/ChallengeBack.Infrastructure/"]

RUN dotnet restore "src/ChallengeBack.Api/ChallengeBack.Api.csproj"

COPY . .

# Build e publish
WORKDIR /src/src/ChallengeBack.Api
RUN dotnet publish "ChallengeBack.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Porta padrão no Cloud Run
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/publish .

# Executar como usuário não-root
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "ChallengeBack.Api.dll"]
