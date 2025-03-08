using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DescriptionsController(IDescriptionRepository descriptionRepository) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DescriptionDto>>> GetDescriptions()
    {
        try
        {
            var descriptions = await descriptionRepository.GetDescriptionsAsync();
            return Ok(descriptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving descriptions: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the descriptions.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DescriptionDto>> GetDescriptionById(int id)
    {
        try
        {
            var description = await descriptionRepository.GetDescriptionByIdAsync(id);
            return Ok(description);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving description with ID {id}: {ex.Message}");
            return StatusCode(500, $"An error occurred while fetching the description with ID {id}.");
        }
    }

    [HttpGet("{id}/name")]
    public async Task<ActionResult<string>> GetDescriptionName(int id)
    {
        try
        {
            var descriptionName = await descriptionRepository.GetDescriptionNameAsync(id);
            return Ok(descriptionName);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving description name for ID {id}: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the description name.");
        }
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<DescriptionDto>>> GetDescriptionsByCategory(int categoryId)
    {
        try
        {
            if (categoryId < 0)
            {
                return BadRequest("Invalid category ID. It must be zero (for all descriptions) or a positive integer.");
            }

            var descriptions = await descriptionRepository.GetDescriptionsByCategoryAsync(categoryId);

            if (descriptions == null || !descriptions.Any())
            {
                return Ok(Enumerable.Empty<DescriptionDto>());
            }

            return Ok(descriptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving descriptions for category ID {categoryId}: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while retrieving descriptions.");
        }
    }


    [HttpPost("add")]
    public async Task<ActionResult> AddDescription([FromBody] DescriptionDto descriptionDto)
    {
        try
        {
            if (descriptionDto == null)
            {
                return BadRequest("Description data is required.");
            }

            Console.WriteLine($"Received Description: {descriptionDto.Name}, CategoryId: {descriptionDto.CategoryId}");

            var newDescription = await descriptionRepository.AddDescriptionAsync(descriptionDto);

            if (newDescription == null)
            {
                return StatusCode(500, "Error adding description.");
            }

            return CreatedAtAction(nameof(GetDescriptionsByCategory), new { categoryId = descriptionDto.CategoryId }, newDescription);
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Validation Error: {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error while adding description: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while adding the description.");
        }
    }
}
