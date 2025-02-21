using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Accounts")]
public class Account
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }

    // Users Relationship (Many-to-Many)
    // An account can belong to multiple users, and a user can have multiple accounts.
    public List<AppUser> Users { get; set; } = [];

    // Debits and Credits Relationship (One-to-Many)
    // One account can be linked to many debits or credits.
    // A single account can be referenced by multiple debit or credit transactions.
    public List<Debit> Debits { get; set; } = [];
    public List<Credit> Credits { get; set; } = [];
}
