using Isopoh.Cryptography.Argon2;
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

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                bool phoneExists = _context.users.Any(p => p.Phone == register.Phone.Trim());
                bool emailExists = _context.users.Any(p => p.Email == register.Email.Trim());

                if (phoneExists || emailExists)
                {
                    if (phoneExists) ModelState.AddModelError("Phone", "این شماره تلفن قبلاً ثبت شده است.");
                    if (emailExists) ModelState.AddModelError("Email", "این ایمیل قبلاً ثبت شده است. لطفاً ایمیل دیگری وارد کنید.");

                    return View(register); // بازگرداندن فرم ثبت‌نام همراه با پیام‌های خطا
                }
                if (!register.AcceptTerms)
                {
                    ModelState.AddModelError("AcceptTerms", "لطفاً قوانین سایت را تأیید کنید.");
                    return View(register);
                }

                // اگر هیچ خطایی وجود نداشت، ثبت‌نام انجام شود
                _context.users.Add(new User
                {
                    FullName = register.FullName,
                    Phone = register.Phone,
                    Email = register.Email,
                    Password = Argon2.Hash(register.Password),
                });
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(register);
        }

    }
}
