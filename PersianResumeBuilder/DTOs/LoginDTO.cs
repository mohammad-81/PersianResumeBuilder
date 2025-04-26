using System.ComponentModel.DataAnnotations;

namespace PersianResumeBuilder.DTOs
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "لطفاً ایمیل یا شماره تلفن را وارد کنید.")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "لطفاً رمز عبور را وارد کنید.")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد.")]
        public string Password { get; set; }
    }
}

