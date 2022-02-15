namespace Model.Dtos.Creation;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Country { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 4)]
    public string Password { get; set; }
}