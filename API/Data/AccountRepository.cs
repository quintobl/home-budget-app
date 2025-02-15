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
        return await context.Accounts
            .ProjectTo<AccountDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        return await context.Accounts
            .Where(a => a.Id == id)
            .ProjectTo<AccountDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}
