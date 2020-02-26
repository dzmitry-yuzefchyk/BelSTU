using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Account
{
    public class RegistrationModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}
