using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<string> GetCategoryNameAsync(int id);
        Task<CategoryDto?> AddCategoryAsync(string categoryName);
        Task<CategoryDto?> DeleteCategoryAsync(int id);
    }
}
