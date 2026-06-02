using System.ComponentModel.DataAnnotations;

namespace Simulation2.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; }
        [MinLength(3)]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
