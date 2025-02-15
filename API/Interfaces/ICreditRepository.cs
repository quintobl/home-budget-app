using API.DTOs;

namespace API.Interfaces;

public interface ICreditRepository
{
    Task<IEnumerable<CreditDto>> GetCreditsAsync();
    Task<CreditDto?> GetCreditByIdAsync(int id);
}
