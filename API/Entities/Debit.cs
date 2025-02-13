using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Debits")]
public class Debit
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int? AccountId { get; set; }
}
