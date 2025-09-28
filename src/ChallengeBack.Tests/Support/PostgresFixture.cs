using Npgsql;
using Testcontainers.PostgreSql;

public sealed class PostgresFixture : IAsyncLifetime, IAsyncDisposable
{
    private readonly PostgreSqlContainer _container;
    private string _connectionString = string.Empty;

    public PostgresFixture()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("challenge_test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public string ConnectionString => _connectionString;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
    }

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();
    async ValueTask IAsyncDisposable.DisposeAsync() => await _container.DisposeAsync();
}
