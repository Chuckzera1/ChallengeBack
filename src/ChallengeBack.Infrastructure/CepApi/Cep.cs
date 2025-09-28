using ChallengeBack.Application.Interfaces.Repositories;

namespace ChallengeBack.Infrastructure.CepApi;

public class CepRepository : ICepRepository
{
    private readonly HttpClient _httpClient;

    public CepRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetByZipCodeAsync(string zipCode)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{zipCode}/json/");
        var content = await response.Content.ReadAsStringAsync();
        var resBody = System.Text.Json.JsonSerializer.Deserialize<dynamic>(content) ?? throw new Exception("CEP não encontrado");
        return resBody.GetProperty("uf").GetString();
    }
}


