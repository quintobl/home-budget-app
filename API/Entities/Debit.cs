using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Debits")]
public class Debit
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    // Foreign Key - Account Relationship (Many-to-One)
    public int? AccountId { get; set; }
    public Account Account { get; set; }

    // Foreign Key - Category Relationship (Many-to-One)
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public string CategoryName { get; set; }
}
