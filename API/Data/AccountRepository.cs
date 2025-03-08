using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AccountRepository(DataContext context, IMapper mapper) : IAccountRepository
{
    public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
    {
        try
        {
            return await context.Accounts
                .ProjectTo<AccountDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving accounts: {ex.Message}");
            throw new Exception("An error occurred while retrieving accounts.", ex);
        }
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        try
        {
            return await context.Accounts
                .Where(a => a.Id == id)
                .ProjectTo<AccountDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving account with ID {id}: {ex.Message}");
            throw new Exception($"An error occurred while retrieving account with ID {id}.", ex);
        }
    }

    public async Task<string> GetAccountNameAsync(int accountId)
    {
        try
        {
            var accountName = await context.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(accountName))
            {
                throw new ArgumentException($"Invalid Account ID: {accountId}.");
            }

            return accountName;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error while retrieving account name for ID {accountId}: {ex.Message}");
            throw new Exception($"An error occurred while retrieving the account name for ID {accountId}.", ex);
        }
    }

}
