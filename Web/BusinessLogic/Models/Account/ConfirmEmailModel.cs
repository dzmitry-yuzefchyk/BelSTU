using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models.Account
{
    public class ConfirmEmailModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
