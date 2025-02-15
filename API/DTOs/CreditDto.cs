namespace API.DTOs;

public class CreditDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? CategoryName { get; set; }
}
