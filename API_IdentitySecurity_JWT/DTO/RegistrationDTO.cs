using System.ComponentModel.DataAnnotations;

namespace API_IdentitySecurity_JWT.DTO
{
    public class RegistrationDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
