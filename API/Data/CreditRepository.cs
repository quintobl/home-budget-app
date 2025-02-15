using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CreditRepository(DataContext context, IMapper mapper) : ICreditRepository
{
    public async Task<IEnumerable<CreditDto>> GetCreditsAsync()
    {
        return await context.Credits
            .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<CreditDto?> GetCreditByIdAsync(int id)
    {
        return await context.Credits
            .Where(c => c.Id == id)
            .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}
