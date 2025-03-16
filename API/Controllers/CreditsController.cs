using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class CreditsController(ICreditRepository creditRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditDto>>> GetCredits()
    {
        try
        {
            var credits = await creditRepository.GetCreditsAsync();

            if (credits == null || !credits.Any())
            {
                return NotFound("No credits found.");
            }

            return Ok(credits);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving credits: {ex.Message}");

            return StatusCode(500, "An unexpected error occurred while retrieving credits.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditDto>> GetCredit(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid credit ID.");
            }

            var credit = await creditRepository.GetCreditByIdAsync(id);

            if (credit == null)
            {
                return NotFound($"Credit with ID {id} not found.");
            }

            return Ok(credit);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving credit ID {id}: {ex.Message}");

            return StatusCode(500, "An unexpected error occurred while retrieving the credit.");
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<CreditDto>> AddCredit([FromBody] CreditDto creditDto)
    {
        try
        {
            var createdCredit = await creditRepository.AddCreditAsync(creditDto);
            return CreatedAtAction(nameof(GetCredit), new { id = createdCredit.Id }, createdCredit);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding credit: {ex.Message}");
            return StatusCode(500, "An error occurred while adding the credit.");
        }
    }

    [HttpGet("by-date")]
    public async Task<ActionResult<IEnumerable<CreditDto>>> GetCreditsByDate()
    {
        var credits = await creditRepository.GetCreditsByDateAsync();
        return Ok(credits);
    }

    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<CreditDto>>> GetCreditsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date cannot be after end date.");
        }

        var credits = await creditRepository.GetCreditsByDateRangeAsync(startDate, endDate);
        return Ok(credits);
    }
}
