using System.ComponentModel.DataAnnotations;

namespace PersianResumeBuilder.DTOs
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "لطفاً ایمیل یا شماره تلفن را وارد کنید.")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

