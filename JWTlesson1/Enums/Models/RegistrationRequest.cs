using System.ComponentModel.DataAnnotations;

namespace JWTlesson1.API.Enums.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
        public Role Role { get; set; }
    }
}
