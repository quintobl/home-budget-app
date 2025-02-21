using System.ComponentModel.DataAnnotations.Schema;
using API.Entities;

[Table("Descriptions")]
public class Description
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // Foreign Key - Category Relationship (Many-to-One)
    // Many descriptions can belong to one category, but each description must be associated with a single category.
    public int CategoryId { get; set; } // Foreign Key linking to Category table
    public Category Category { get; set; } // Navigation property
    public string CategoryName { get; set; } // Displays the category name

    // Debits and Credits Relationship (One-to-Many)
    // One description can be linked to many debits or credits.
    // A single description can be referenced by multiple debit or credit transactions.
    public List<Debit> Debits { get; set; } = [];
    public List<Credit> Credits { get; set; } = [];
}
