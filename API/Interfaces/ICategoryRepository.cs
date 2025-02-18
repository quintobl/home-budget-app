using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto?> AddCategoryAsync(CategoryDto categoryDto);
        Task<CategoryDto?> DeleteCategoryAsync(int id);
        Task<bool> SaveAllAsync();
    }
}
