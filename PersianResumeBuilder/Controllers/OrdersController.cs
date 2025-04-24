using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PersianResumeBuilder.DataBase;
using PersianResumeBuilder.DTOs;
using PersianResumeBuilder.Entities;

namespace PersianResumeBuilder.Controllers
{
    public class OrdersController : Controller
    {
        private readonly Sample_DbContext _context;
        public OrdersController(Sample_DbContext context)
        {
            _context = context;
        }
        public IActionResult Orders()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResultOrder()
        {
            return View();

        }

        [HttpPost]
        public IActionResult ResultOrder(Customer model)
        {
            _context.Customers.Add(model);
            _context.SaveChanges();
            return View();

        }
    }
}
