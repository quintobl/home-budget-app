namespace API.DTOs;

public class CreditDto
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string? AccountName { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CustomCategory { get; set; }
    public int DescriptionId { get; set; }
    public string? DescriptionName { get; set; }
    public string? CustomDescription { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

}
