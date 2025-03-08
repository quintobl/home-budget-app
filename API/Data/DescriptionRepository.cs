using API.DTOs;
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
            try
            {
                return await context.Descriptions
                    .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving descriptions: {ex.Message}");
                throw new Exception("An error occurred while retrieving descriptions.", ex);
            }
        }

        public async Task<DescriptionDto?> GetDescriptionByIdAsync(int id)
        {
            try
            {
                return await context.Descriptions
                    .Where(d => d.Id == id)
                    .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving description with ID {id}: {ex.Message}");
                throw new Exception($"An error occurred while retrieving the description with ID {id}.", ex);
            }
        }

        public async Task<string> GetDescriptionNameAsync(int descriptionId)
        {
            try
            {
                var descriptionName = await context.Descriptions
                    .Where(d => d.Id == descriptionId)
                    .Select(d => d.Name)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(descriptionName))
                {
                    throw new ArgumentException($"Invalid Description ID: {descriptionId}.");
                }

                return descriptionName;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error while retrieving description name for ID {descriptionId}: {ex.Message}");
                throw new Exception($"An error occurred while retrieving the description name for ID {descriptionId}.", ex);
            }
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId < 0)
                {
                    throw new ArgumentException("Invalid category ID. It must be zero (for all descriptions) or a positive integer.", nameof(categoryId));
                }

                IQueryable<Description> query = context.Descriptions;

                if (categoryId > 0)
                {
                    query = query.Where(d => d.CategoryId == categoryId);
                }

                var descriptions = await query
                    .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return descriptions ?? Enumerable.Empty<DescriptionDto>();
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Validation Error: {ex.Message}");
                throw new ApplicationException("Invalid category ID provided.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while retrieving descriptions for category ID {categoryId}: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving descriptions.", ex);
            }
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    throw new ArgumentException("Start date must be before or equal to the end date.", nameof(startDate));
                }

                var descriptions = await context.Descriptions
                    .Where(d => d.Debits.Any(debit => debit.Date >= startDate && debit.Date <= endDate) ||
                                d.Credits.Any(credit => credit.Date >= startDate && credit.Date <= endDate))
                    .ProjectTo<DescriptionDto>(mapper.ConfigurationProvider)
                    .ToListAsync();

                if (descriptions == null || !descriptions.Any())
                {
                    Console.Error.WriteLine($"No descriptions found within the date range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}.");
                    return Enumerable.Empty<DescriptionDto>();
                }

                return descriptions;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Validation Error: {ex.Message}");
                throw new ApplicationException("Invalid date range provided.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while retrieving descriptions for date range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}: {ex.Message}");
                throw new ApplicationException("An error occurred while retrieving descriptions.", ex);
            }
        }

        public async Task<DescriptionDto?> AddDescriptionAsync(DescriptionDto descriptionDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descriptionDto.Name))
                {
                    throw new ArgumentException("Description name is required.", nameof(descriptionDto.Name));
                }

                if (descriptionDto.CategoryId <= 0)
                {
                    throw new ArgumentException("Invalid Category ID. It must be a positive integer.", nameof(descriptionDto.CategoryId));
                }

                var categoryExists = await context.Categories.AnyAsync(c => c.Id == descriptionDto.CategoryId);
                if (!categoryExists)
                {
                    throw new ArgumentException($"Category ID {descriptionDto.CategoryId} does not exist.");
                }

                var newDescription = new Description
                {
                    Name = descriptionDto.Name,
                    CategoryId = descriptionDto.CategoryId
                };

                await context.Descriptions.AddAsync(newDescription);
                await context.SaveChangesAsync();

                return mapper.Map<DescriptionDto>(newDescription);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Validation Error: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.Error.WriteLine($"Database Error: {ex.InnerException?.Message ?? ex.Message}");
                throw new ApplicationException("An error occurred while saving the description.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while adding description: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while adding the description.", ex);
            }
        }

        public async Task<DescriptionDto?> DeleteDescriptionAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid description ID. ID must be a positive integer.", nameof(id));
                }

                var description = await context.Descriptions.FindAsync(id);

                if (description == null)
                {
                    Console.Error.WriteLine($"Description with ID {id} not found.");
                    return null;
                }

                context.Descriptions.Remove(description);
                var success = await context.SaveChangesAsync() > 0;

                if (!success)
                {
                    throw new InvalidOperationException($"Failed to delete description with ID {id}.");
                }

                return mapper.Map<DescriptionDto>(description);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Validation Error: {ex.Message}");
                throw new ApplicationException("Invalid description ID provided.", ex);
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Database Error: {ex.Message}");
                throw new ApplicationException("An error occurred while deleting the description.", ex);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error while deleting description ID {id}: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred while deleting the description.", ex);
            }
        }

    }
}
