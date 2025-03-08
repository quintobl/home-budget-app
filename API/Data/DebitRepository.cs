using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DebitRepository(DataContext context, IMapper mapper, IAccountRepository accountRepository, ICategoryRepository categoryRepository, IDescriptionRepository descriptionRepository) : IDebitRepository
{
    public async Task<IEnumerable<DebitDto>> GetDebitsAsync()
    {
        try
        {
            var debits = await context.Debits
                .ProjectTo<DebitDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            if (debits == null || !debits.Any())
            {
                Console.Error.WriteLine("No debits found.");
                return Enumerable.Empty<DebitDto>();
            }

            return debits;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while retrieving debits: {ex.Message}");
            throw new ApplicationException("An error occurred while retrieving the list of debits.", ex);
        }
    }

    public async Task<DebitDto?> GetDebitByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid debit ID.", nameof(id));
            }

            var debit = await context.Debits
                .Where(d => d.Id == id)
                .ProjectTo<DebitDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (debit == null)
            {
                Console.Error.WriteLine($"Debit with ID {id} not found.");
                return null;
            }

            return debit;
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Validation Error: {ex.Message}");
            throw new ApplicationException("Invalid input data provided.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while retrieving debit ID {id}: {ex.Message}");
            throw new ApplicationException("An error occurred while retrieving the debit.", ex);
        }
    }

    public async Task<DebitDto> AddDebitAsync(DebitDto debitDto)
    {
        try
        {
            ValidateDebitDto(debitDto);

            debitDto.AccountName = await EnsureAccountExistsAsync(debitDto.AccountId);
            debitDto.CategoryId = await EnsureCategoryExistsAsync(debitDto.CategoryId, debitDto.CustomCategory);
            debitDto.DescriptionId = await EnsureDescriptionExistsAsync(debitDto.DescriptionId, debitDto.CustomDescription, debitDto.CategoryId);
            debitDto.CategoryName = await categoryRepository.GetCategoryNameAsync(debitDto.CategoryId);
            debitDto.DescriptionName = await descriptionRepository.GetDescriptionNameAsync(debitDto.DescriptionId);

            return await InsertDebitAsync(debitDto);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while adding debit: {ex.Message}");
            throw new ApplicationException("An error occurred while processing the debit transaction.", ex);
        }
    }

    private void ValidateDebitDto(DebitDto debitDto)
    {
        if (debitDto == null)
        {
            throw new ArgumentNullException(nameof(debitDto), "DebitDto cannot be null.");
        }

        if (debitDto.AccountId <= 0)
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

    private async Task<DebitDto> InsertDebitAsync(DebitDto debitDto)
    {
        var debit = mapper.Map<Debit>(debitDto);

        try
        {
            context.Debits.Add(debit);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            throw new ApplicationException("An error occurred while saving the debit transaction.", ex);
        }

        return mapper.Map<DebitDto>(debit);
    }
}