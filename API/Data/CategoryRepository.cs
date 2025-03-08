using API.DTOs;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CategoryRepository(DataContext context, IMapper mapper) : ICategoryRepository
{

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            return await context.Categories
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving categories: {ex.Message}");
            throw new Exception("An error occurred while retrieving categories.", ex);
        }
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        try
        {
            return await context.Categories
                .Where(a => a.Id == id)
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving category with ID {id}: {ex.Message}");
            throw new Exception($"An error occurred while retrieving category with ID {id}.", ex);
        }
    }

    public async Task<string> GetCategoryNameAsync(int categoryId)
    {
        try
        {
            var categoryName = await context.Categories
                .Where(c => c.Id == categoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentException($"Invalid Category ID: {categoryId}.");
            }

            return categoryName;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error while retrieving category name for ID {categoryId}: {ex.Message}");
            throw new Exception($"An error occurred while retrieving the category name for ID {categoryId}.", ex);
        }
    }

    public async Task<CategoryDto?> AddCategoryAsync(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new ArgumentException("Category name cannot be empty.", nameof(categoryName));
        }

        var newCategory = new Category
        {
            Name = categoryName
        };

        try
        {
            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            };
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Database update failed: {ex.Message}");

            throw new InvalidOperationException("An error occurred while saving the category. Please try again later.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");

            throw new ApplicationException("An unexpected error occurred while adding the category.", ex);
        }
    }

    public async Task<CategoryDto?> DeleteCategoryAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid category ID.", nameof(id));
            }

            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }

            context.Categories.Remove(category);
            var success = await context.SaveChangesAsync() > 0;

            if (!success)
            {
                throw new InvalidOperationException("Failed to delete the category.");
            }

            return mapper.Map<CategoryDto>(category);
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Validation Error: {ex.Message}");

            return null;
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"Database Error: {ex.Message}");

            return null;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while deleting category ID {id}: {ex.Message}");

            return null;
        }
    }

}