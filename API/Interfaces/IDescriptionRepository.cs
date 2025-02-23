using API.DTOs;

namespace API.Interfaces
{
    public interface IDescriptionRepository
    {
        Task<IEnumerable<DescriptionDto>> GetDescriptionsAsync();
        Task<DescriptionDto?> GetDescriptionByIdAsync(int id);
        Task<DescriptionDto?> AddDescriptionAsync(DescriptionDto descriptionDto);
        Task<DescriptionDto?> DeleteDescriptionAsync(int id);
        Task<IEnumerable<DescriptionDto>> GetDescriptionsByCategoryAsync(int categoryId);
        Task<IEnumerable<DescriptionDto>> GetDescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> SaveAllAsync();
    }
}
