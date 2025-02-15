using API.DTOs;

namespace API.Interfaces;

public interface IDebitRepository
{
    Task<IEnumerable<DebitDto>> GetDebitsAsync();
    Task<DebitDto?> GetDebitByIdAsync(int id);
}
