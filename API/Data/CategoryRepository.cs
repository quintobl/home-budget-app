using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CategoryRepository(DataContext context, IMapper mapper) : ICategoryRepository
{

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        return await context.Categories
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        return await context.Categories
            .Where(a => a.Id == id)
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<CategoryDto?> AddCategoryAsync(CategoryDto categoryDto)
    {
        var category = mapper.Map<Category>(categoryDto);

        await context.Categories.AddAsync(category);
        var success = await context.SaveChangesAsync() > 0;

        if (!success) return null;

        return await context.Categories
            .Where(c => c.Id == category.Id)
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<CategoryDto?> DeleteCategoryAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);

        if (category == null) return null;

        context.Categories.Remove(category);
        var success = await context.SaveChangesAsync() > 0;

        if (!success) return null;

        return mapper.Map<CategoryDto>(category);
    }
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

}
