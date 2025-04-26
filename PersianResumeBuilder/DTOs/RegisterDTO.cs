using System.ComponentModel.DataAnnotations;

namespace PersianResumeBuilder.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="رمز وارد شده یکسان نیست")]
        public string Re_Password { get; set; }
        [Required(ErrorMessage ="لطفا قوانین سایت رو تایید کنید")]
        public bool AcceptTerms { get; set; }

    }
}
