using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class CategoriesController(ICategoryRepository categoryRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving categories: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the categories.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        try
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);
            if (category == null) return NotFound($"Category with ID {id} not found.");
            return Ok(category);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving category with ID {id}: {ex.Message}");
            return StatusCode(500, $"An error occurred while fetching the category with ID {id}.");
        }
    }

    [HttpGet("{id}/name")]
    public async Task<ActionResult<string>> GetCategoryName(int id)
    {
        try
        {
            var categoryName = await categoryRepository.GetCategoryNameAsync(id);
            return Ok(categoryName);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving category name for ID {id}: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the category name.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            var deletedCategory = await categoryRepository.DeleteCategoryAsync(id);

            if (deletedCategory == null)
            {
                return NotFound($"Category with ID {id} not found or could not be deleted.");
            }

            return Ok(deletedCategory);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting category ID {id}: {ex.Message}");

            return StatusCode(500, "An unexpected error occurred while deleting the category.");
        }
    }


    [HttpPost("add")]
    public async Task<ActionResult> AddCategory([FromBody] CategoryDto categoryDto)
    {
        try
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                return BadRequest("Category name is required.");
            }

            var newCategory = await categoryRepository.AddCategoryAsync(categoryDto.Name);

            if (newCategory == null)
            {
                return StatusCode(500, "An error occurred while creating the category.");
            }

            return CreatedAtAction(nameof(GetCategoryName), new { id = newCategory.Id }, newCategory);
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Validation Error: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while adding category: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while adding the category.");
        }
    }
}
