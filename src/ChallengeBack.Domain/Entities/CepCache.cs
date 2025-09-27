namespace ChallengeBack.Domain.Entities;

public class CepCache
{

    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Localidade { get; private set; }
    public string Uf { get; private set; }
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public CepCache() { }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}