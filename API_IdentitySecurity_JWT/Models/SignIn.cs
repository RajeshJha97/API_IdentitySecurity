using System.ComponentModel.DataAnnotations;

namespace API_IdentitySecurity_JWT.Models
{
    public class SignIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
