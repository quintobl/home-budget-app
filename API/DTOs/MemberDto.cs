namespace API.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public DateTime Created { get; set; }
    public List<AccountDto>? Accounts { get; set; }
}