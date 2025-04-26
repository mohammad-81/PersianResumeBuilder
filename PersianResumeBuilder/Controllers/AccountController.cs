using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianResumeBuilder.DataBase;
using PersianResumeBuilder.DTOs;
using PersianResumeBuilder.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

            var user = await _context.users
                .FirstOrDefaultAsync(p => p.Email == login.EmailOrPhone || p.Phone == login.EmailOrPhone);

            if (user == null)
            {
                ModelState.AddModelError("EmailOrPhone", "کاربری با این اطلاعات یافت نشد.");
                return View(login);
            }

            // بررسی اینکه آیا رمز عبور هش شده است یا خیر
            bool isHashedPassword = user.Password.StartsWith("$argon2");

            if (isHashedPassword)
            {
                // بررسی رمز عبور هش شده
                if (!Argon2.Verify(user.Password, login.Password))
                {
                    ModelState.AddModelError("Password", "رمز عبور اشتباه است.");
                    return View(login);
                }
            }
            else
            {
                // بررسی رمز عبور بدون هش
                if (user.Password != login.Password)
                {
                    ModelState.AddModelError("Password", "رمز عبور اشتباه است.");
                    return View(login);
                }

                // هش کردن رمز عبور و ذخیره آن در دیتابیس
                user.Password = Argon2.Hash(login.Password);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            // احراز هویت با کوکی
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #endregion
    }
}
