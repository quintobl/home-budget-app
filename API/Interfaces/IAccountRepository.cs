using API.DTOs;

namespace API.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<AccountDto>> GetAccountsAsync();
    Task<AccountDto?> GetAccountByIdAsync(int id);
    Task<string> GetAccountNameAsync(int accountId);
}
