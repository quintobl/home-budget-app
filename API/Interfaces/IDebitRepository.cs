using API.DTOs;

namespace API.Interfaces;

public interface IDebitRepository
{
    Task<IEnumerable<DebitDto>> GetDebitsAsync();
    Task<DebitDto?> GetDebitByIdAsync(int id);
    Task<int> GetNextDescriptionIdAsync();
    Task AddDescriptionAsync(string descriptionName);
    Task<DebitDto> AddDebitAsync(DebitDto debitDto);

}
