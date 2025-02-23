namespace API.DTOs;

public class DebitDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? CategoryName { get; set; }
    public int DescriptionId { get; set; }
    public string? DescriptionName { get; set; }
}
