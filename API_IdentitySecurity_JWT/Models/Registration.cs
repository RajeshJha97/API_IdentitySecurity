using System.ComponentModel.DataAnnotations;

namespace API_IdentitySecurity_JWT.Models
{
    public class Registration
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public DateTime RegistrationTime { get; set; }=DateTime.Now;
        
    }
}
