using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AccountsController(IAccountRepository accountRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
    {
        try
        {
            var accounts = await accountRepository.GetAccountsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving accounts: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving accounts.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid account ID.");
            }

            var account = await accountRepository.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID {id} not found.");
            }

            return Ok(account);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving account ID {id}: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving the account.");
        }
    }


    [HttpGet("{id}/name")]
    public async Task<ActionResult<string>> GetAccountName(int id)
    {
        try
        {
            var accountName = await accountRepository.GetAccountNameAsync(id);
            return Ok(accountName);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving account name: {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the account name.");
        }
    }
}
