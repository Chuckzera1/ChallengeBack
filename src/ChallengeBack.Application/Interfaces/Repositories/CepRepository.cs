namespace ChallengeBack.Application.Interfaces.Repositories;

public interface ICepRepository
{
    Task<string> GetByZipCodeAsync(string zipCode);
}
