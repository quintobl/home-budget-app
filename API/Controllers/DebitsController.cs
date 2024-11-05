using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API;

[ApiController]
[Route("api/[controller]")]
public class DebitsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Debit>> GetDebits()
    {
        var debits = context.Debits.ToList();

        return debits;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Debit> GetDebit(int id)
    {
        var debit = context.Debits.Find(id);

        if (debit == null)
        {
            return NotFound();
        }
        return debit;
    }
}