using AuthenticationAPIApp.Contracts;
using AuthenticationAPIApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPIApp.Model
{
    public class Login : Ilogin
    {
        [Required]
        [NotEmpty]
        public string UserName { get; set; }
        [Required]
        [NotEmpty]
        public string Password { get; set; }
    }
}
