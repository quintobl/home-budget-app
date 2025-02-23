using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<DebitDto> AddDebitAsync([FromBody] DebitDto debitDto)
    {
        var debit = mapper.Map<Debit>(debitDto);

        context.Debits.Add(debit);
        await context.SaveChangesAsync();

        return mapper.Map<DebitDto>(debit);
    }

    public async Task<int> GetNextDescriptionIdAsync()
    {
        int maxId = await context.Descriptions.MaxAsync(d => (int?)d.Id) ?? 0;

        int newId = maxId + 1;
        while (await context.Descriptions.AnyAsync(d => d.Id == newId))
        {
            newId++;
        }

        return newId;
    }


    public async Task AddDescriptionAsync(string descriptionName)
    {
        if (string.IsNullOrWhiteSpace(descriptionName))
        {
            throw new ArgumentException("Description name cannot be empty.");
        }

        var newDescription = new Description
        {
            Name = descriptionName
        };

        try
        {
            await context.Descriptions.AddAsync(newDescription);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error inserting description: {ex.InnerException?.Message ?? ex.Message}");
            throw;
        }
    }

}
