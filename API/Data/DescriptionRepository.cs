using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DescriptionRepository(DataContext context, IMapper mapper) : IDescriptionRepository
    {
        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsAsync()
        {
            return await context.Descriptions
                .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<DescriptionDto?> GetDescriptionByIdAsync(int id)
        {
            return await context.Descriptions
                .Where(d => d.Id == id)
                .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByCategoryAsync(int categoryId)
        {
            return await context.Descriptions
                .Where(d => d.CategoryId == categoryId)
                .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await context.Descriptions
                .Where(d => d.Debits.Any(debit => debit.Date >= startDate && debit.Date <= endDate) ||
                            d.Credits.Any(credit => credit.Date >= startDate && credit.Date <= endDate))
                .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<DescriptionDto?> AddDescriptionAsync(DescriptionDto descriptionDto)
        {
            throw new NotImplementedException();
        }

        public Task<DescriptionDto?> DeleteDescriptionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
