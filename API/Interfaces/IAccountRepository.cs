using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<AccountDto>> GetAccountsAsync();
    Task<AccountDto?> GetAccountByIdAsync(int id);
}
