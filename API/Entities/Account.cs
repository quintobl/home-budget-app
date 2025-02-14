using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Accounts")]
public class Account
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }

    // Many-to-Many: An account can belong to multiple users
    public List<AppUser> Users { get; set; } = [];

    // One-to-Many: An account has multiple debit and credit transactions
    public List<Debit> Debits { get; set; } = [];
    public List<Credit> Credits { get; set; } = [];
}
