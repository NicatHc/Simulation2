using System.ComponentModel.DataAnnotations;

namespace Simulation2.ViewModels.Account
{
    public class LoginVM
    {
        [MaxLength(256, ErrorMessage = "uzundur")]
        public string UserNameOrEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
