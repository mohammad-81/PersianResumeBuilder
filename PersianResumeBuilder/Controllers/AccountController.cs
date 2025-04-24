using Microsoft.AspNetCore.Mvc;
using PersianResumeBuilder.DataBase;
using PersianResumeBuilder.DTOs;
using PersianResumeBuilder.Entities;

namespace PersianResumeBuilder.Controllers
{
    public class AccountController : Controller
    {
        private readonly Sample_DbContext _context;
        public AccountController(Sample_DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO register)
        {
            if (ModelState.IsValid) 
            {
                if (!_context.users.Any(p => p.Phone == register.Phone.Trim()))
                {
                    User user = new User()
                    {
                        FullName = register.FullName,
                        Phone = register.Phone,
                        Email = register.Email,
                        Password = register.Password,

                    };
                }
            }
            return View();
        }
    }
}
