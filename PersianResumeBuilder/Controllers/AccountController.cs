using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianResumeBuilder.DataBase;
using PersianResumeBuilder.DTOs;
using PersianResumeBuilder.Entities;
using System.Security.Claims;

namespace PersianResumeBuilder.Controllers
{
    public class AccountController : Controller
    {
        private readonly Sample_DbContext _context;
        public AccountController(Sample_DbContext context)
        {
            _context = context;
        }

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
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

        #endregion
        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            // استفاده از Asynchronous Query
            var user = await _context.users
                .FirstOrDefaultAsync(p => p.Email == login.EmailOrPhone || p.Phone == login.EmailOrPhone);

            if (user == null)
            {
                ModelState.AddModelError("EmailOrPhone", "کاربری با این اطلاعات یافت نشد.");
                return View(login);
            }

            // بررسی رمز عبور
            if (!Argon2.Verify(user.Password, login.Password))
            {
                ModelState.AddModelError("Password", "رمز عبور اشتباه است.");
                return View(login);
            }

            // احراز هویت با کوکی، به صورت Async
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index","Home");
        }
        #endregion
    }
}
