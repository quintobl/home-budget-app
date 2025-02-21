using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Debits")]
public class Debit
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    // Foreign Key - Account Relationship (Many-to-One)
    // Many debits can belong to one account, but each debit must be associated with a single account.
    public int AccountId { get; set; } // Foreign Key linking to Account table
    public Account Account { get; set; } // Navigation property
    public string AccountName { get; set; } // Displays the account name

    // Foreign Key - Category Relationship (Many-to-One)
    // Many debits can belong to one category, but each debit must be associated with a single category.
    public int CategoryId { get; set; } // Foreign Key linking to Category table
    public Category Category { get; set; } // Navigation property
    public string CategoryName { get; set; } // Displays the category name

    // Foreign Key - Description Relationship (Many-to-One)
    // Many debits can share the same description, but each debit must be associated with a single description.
    public int DescriptionId { get; set; } // Foreign Key linking to Description table
    public Description Description { get; set; } // Navigation property
    public string DescriptionName { get; set; } // Displays the description name
}
