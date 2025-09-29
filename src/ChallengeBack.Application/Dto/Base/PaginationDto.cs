namespace ChallengeBack.Application.Dto.Base;

public class PaginationDto
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    
    public int Skip => (Page - 1) * Limit;
}
