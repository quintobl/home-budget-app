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
}
