using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DebitsController(IDebitRepository debitRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DebitDto>>> GetDebits()
    {
        try
        {
            var debits = await debitRepository.GetDebitsAsync();

            if (debits == null || !debits.Any())
            {
                return NotFound("No debits found.");
            }

            return Ok(debits);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving debits: {ex.Message}");

            return StatusCode(500, "An unexpected error occurred while retrieving debits.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DebitDto>> GetDebit(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid debit ID.");
            }

            var debit = await debitRepository.GetDebitByIdAsync(id);

            if (debit == null)
            {
                return NotFound($"Debit with ID {id} not found.");
            }

            return Ok(debit);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving debit ID {id}: {ex.Message}");

            return StatusCode(500, "An unexpected error occurred while retrieving the debit.");
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<DebitDto>> AddDebit([FromBody] DebitDto debitDto)
    {
        try
        {
            var createdDebit = await debitRepository.AddDebitAsync(debitDto);
            return CreatedAtAction(nameof(GetDebit), new { id = createdDebit.Id }, createdDebit);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding debit: {ex.Message}");
            return StatusCode(500, "An error occurred while adding the debit.");
        }
    }
}
