using System.ComponentModel.DataAnnotations.Schema;
using API.Entities;

[Table("Categories")]
public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // One-to-Many: A category can be assigned to multiple transactions
    public List<Debit> Debits { get; set; } = [];
    public List<Credit> Credits { get; set; } = [];
}
