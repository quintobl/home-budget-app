using API.DTOs;

namespace API.Interfaces;

public interface ICreditRepository
{
    Task<IEnumerable<CreditDto>> GetCreditsAsync();
    Task<CreditDto?> GetCreditByIdAsync(int id);
    Task<IEnumerable<CreditDto>> GetCreditsByDateAsync();
    Task<IEnumerable<CreditDto>> GetCreditsByDateRangeAsync(DateTime startDate, DateTime endDate);
}
