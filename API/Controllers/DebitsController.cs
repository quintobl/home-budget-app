using System.Diagnostics;
using API.DTOs;
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
        var debits = await debitRepository.GetDebitsAsync();
        return Ok(debits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DebitDto>> GetDebit(int id)
    {
        var debit = await debitRepository.GetDebitByIdAsync(id);
        if (debit == null) return NotFound();
        return debit;
    }

    [HttpPost("add")]
    public async Task<ActionResult<DebitDto>> AddDebit([FromBody] DebitDto debitDto)
    {
        if (debitDto == null)
        {
            return BadRequest("Debit data is required.");
        }
        if (debitDto.DescriptionId == -1)
        {
            // if (string.IsNullOrEmpty(debitDto.DescriptionName))
            // {
            //     return BadRequest("Custom description is required when DescriptionId is -1.");
            // }

            // Generate a new DescriptionId
            int newDescriptionId = await debitRepository.GetNextDescriptionIdAsync();
            debitDto.DescriptionId = newDescriptionId;

            // Save the new description to the database
            await debitRepository.AddDescriptionAsync(debitDto.DescriptionName);
        }

        var createdDebit = await debitRepository.AddDebitAsync(debitDto);

        return CreatedAtAction(nameof(GetDebit), new { id = createdDebit.Id }, createdDebit);
    }


}
