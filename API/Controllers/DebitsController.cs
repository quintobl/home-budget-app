using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[ApiController]
[Route("api/[controller]")]
public class DebitsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Debit>>> GetDebits()
    {
        var debits = await context.Debits.ToListAsync();

        return debits;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Debit>> GetDebit(int id)
    {
        var debit = await context.Debits.FindAsync(id);

        if (debit == null)
        {
            return NotFound();
        }
        return debit;
    }
}