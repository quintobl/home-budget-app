using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Accounts")]
public class Account
{
    public int AccountId { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }
    public List<AppUser> Users { get; set; } = [];
    //public List<UserAccount> UserAccounts { get; set; } = [];
}
