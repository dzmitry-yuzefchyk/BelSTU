using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
}
