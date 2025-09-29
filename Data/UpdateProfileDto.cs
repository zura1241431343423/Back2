using System.ComponentModel.DataAnnotations;

public class UpdateProfileDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string LastName { get; set; }

    public string? Address { get; set; }
}
