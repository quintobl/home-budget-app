namespace API.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }
    public List<CreditDto>? Credits { get; set; }
    public List<DebitDto>? Debits { get; set; }
}
