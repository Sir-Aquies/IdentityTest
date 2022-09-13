 #nullable disable
using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Models
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
