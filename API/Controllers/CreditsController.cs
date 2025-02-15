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
}
