using System.ComponentModel.DataAnnotations;

namespace API_IdentitySecurity_JWT.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
