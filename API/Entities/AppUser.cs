using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Users")]
public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public List<Account> Accounts { get; set; } = [];
    //public List<UserAccount> UserAccounts { get; set; } = [];
}