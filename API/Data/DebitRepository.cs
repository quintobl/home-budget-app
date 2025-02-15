using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DebitRepository(DataContext context, IMapper mapper) : IDebitRepository
{
    public async Task<IEnumerable<DebitDto>> GetDebitsAsync()
    {
        return await context.Debits
            .ProjectTo<DebitDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<DebitDto?> GetDebitByIdAsync(int id)
    {
        return await context.Debits
            .Where(d => d.Id == id)
            .ProjectTo<DebitDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}
