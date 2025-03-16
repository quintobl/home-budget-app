using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CreditRepository(DataContext context, IMapper mapper, IAccountRepository accountRepository, ICategoryRepository categoryRepository, IDescriptionRepository descriptionRepository) : ICreditRepository
{
    public async Task<IEnumerable<CreditDto>> GetCreditsAsync()
    {
        try
        {
            var credits = await context.Credits
                .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (credits == null || !credits.Any())
            {
                Console.Error.WriteLine("No credits found.");
                return Enumerable.Empty<CreditDto>();
            }

            return credits;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while retrieving credits: {ex.Message}");
            throw new ApplicationException("An error occurred while retrieving the list of credits.", ex);
        }
    }

    public async Task<CreditDto?> GetCreditByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid credit ID.", nameof(id));
            }

            var credit = await context.Credits
                .Where(d => d.Id == id)
                .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (credit == null)
            {
                Console.Error.WriteLine($"Credit with ID {id} not found.");
                return null;
            }

            return credit;
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Validation Error: {ex.Message}");
            throw new ApplicationException("Invalid input data provided.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while retrieving credit ID {id}: {ex.Message}");
            throw new ApplicationException("An error occurred while retrieving the credit.", ex);
        }
    }

    public async Task<CreditDto> AddCreditAsync(CreditDto creditDto)
    {
        try
        {
            ValidateCreditDto(creditDto);

            creditDto.AccountName = await EnsureAccountExistsAsync(creditDto.AccountId);
            creditDto.CategoryId = await EnsureCategoryExistsAsync(creditDto.CategoryId, creditDto.CustomCategory);
            creditDto.DescriptionId = await EnsureDescriptionExistsAsync(creditDto.DescriptionId, creditDto.CustomDescription, creditDto.CategoryId);
            creditDto.CategoryName = await categoryRepository.GetCategoryNameAsync(creditDto.CategoryId);
            creditDto.DescriptionName = await descriptionRepository.GetDescriptionNameAsync(creditDto.DescriptionId);

            return await InsertCreditAsync(creditDto);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while adding credit: {ex.Message}");
            throw new ApplicationException("An error occurred while processing the credit transaction.", ex);
        }
    }

    private void ValidateCreditDto(CreditDto creditDto)
    {
        if (creditDto == null)
        {
            throw new ArgumentNullException(nameof(creditDto), "CreditDto cannot be null.");
        }

        if (creditDto.AccountId <= 0)
        {
            throw new ArgumentException("Invalid Account ID.");
        }
    }

    private async Task<string> EnsureAccountExistsAsync(int accountId)
    {
        var accountName = await accountRepository.GetAccountNameAsync(accountId);

        if (string.IsNullOrEmpty(accountName))
        {
            accountName = await context.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(accountName))
            {
                throw new Exception("Invalid Account ID. AccountName cannot be null.");
            }
        }
        return accountName;
    }

    private async Task<int> EnsureCategoryExistsAsync(int categoryId, string? customCategory)
    {
        if (!string.IsNullOrEmpty(customCategory))
        {
            var newCategory = await categoryRepository.AddCategoryAsync(customCategory);
            if (newCategory == null)
            {
                throw new InvalidOperationException("Failed to create new category.");
            }
            return newCategory.Id;
        }

        var categoryName = await categoryRepository.GetCategoryNameAsync(categoryId);
        if (string.IsNullOrEmpty(categoryName))
        {
            throw new Exception("Invalid Category ID. CategoryName cannot be null.");
        }
        return categoryId;
    }

    private async Task<int> EnsureDescriptionExistsAsync(int descriptionId, string? customDescription, int categoryId)
    {
        if (descriptionId == -1 && !string.IsNullOrEmpty(customDescription))
        {
            var newDescriptionDto = new DescriptionDto
            {
                Name = customDescription,
                CategoryId = categoryId
            };

            var newDescription = await descriptionRepository.AddDescriptionAsync(newDescriptionDto);
            if (newDescription == null)
            {
                throw new Exception("Failed to create new description.");
            }

            return newDescription.Id;
        }

        var descriptionName = await descriptionRepository.GetDescriptionNameAsync(descriptionId);
        if (string.IsNullOrEmpty(descriptionName))
        {
            throw new Exception("Invalid Description ID. DescriptionName cannot be null.");
        }

        bool descriptionExists = await context.Descriptions.AnyAsync(d => d.Id == descriptionId);
        if (!descriptionExists)
        {
            throw new Exception($"Invalid Description ID: {descriptionId}. It does not exist in the Descriptions table.");
        }

        return descriptionId;
    }

    private async Task<CreditDto> InsertCreditAsync(CreditDto creditDto)
    {
        var credit = mapper.Map<Credit>(creditDto);

        try
        {
            context.Credits.Add(credit);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            throw new ApplicationException("An error occurred while saving the credit transaction.", ex);
        }

        return mapper.Map<CreditDto>(credit);
    }

    public async Task<IEnumerable<CreditDto>> GetCreditsByDateAsync()
    {
        return await context.Credits
            .OrderByDescending(c => c.Date)
            .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<CreditDto>> GetCreditsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await context.Credits
            .Where(c => c.Date >= startDate && c.Date <= endDate)
            .OrderByDescending(c => c.Date)
            .ProjectTo<CreditDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
