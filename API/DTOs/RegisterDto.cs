using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}