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
        var credits = await creditRepository.GetCreditsAsync();
        return Ok(credits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditDto>> GetCredit(int id)
    {
        var credit = await creditRepository.GetCreditByIdAsync(id);
        if (credit == null) return NotFound();
        return credit;
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
