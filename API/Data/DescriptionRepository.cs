using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DescriptionRepository : IDescriptionRepository
    {
        // Todo: Change all repositories to have encapsulation like below with backing fields.
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DescriptionRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsAsync()
        {
            return await _context.Descriptions
                .ProjectTo<DescriptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<DescriptionDto?> GetDescriptionByIdAsync(int id)
        {
            return await _context.Descriptions
                .Where(d => d.Id == id)
                .ProjectTo<DescriptionDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByCategoryAsync(int categoryId)
        {
            return await _context.Descriptions
                .Where(d => d.CategoryId == categoryId)
                .ProjectTo<DescriptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Descriptions
                .Where(d => d.Debits.Any(debit => debit.Date >= startDate && debit.Date <= endDate) ||
                            d.Credits.Any(credit => credit.Date >= startDate && credit.Date <= endDate))
                .ProjectTo<DescriptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
