using Microsoft.AspNetCore.Mvc;
using PersianResumeBuilder.DataBase;
using PersianResumeBuilder.DTOs;
using PersianResumeBuilder.Entities;

namespace PersianResumeBuilder.Controllers
{
    public class CustomerProfileController : Controller
    {
        private readonly Sample_DbContext _context;
        public CustomerProfileController(Sample_DbContext context)
        {
            _context = context;
        }
        public IActionResult ShowCustomerProfile(InformationCustomerProfile model)
        {
            List<InformationCustomerProfile>informationCustomerProfiles = new List<InformationCustomerProfile>();
            #region Object Mapping Customer 1
            InformationCustomerProfile mohammadSallamat = new InformationCustomerProfile()
            {
                profileImage= "Student.jpg",
                Name ="MohammadSallamat",
                JobPosition="دانشجو",
                Age=23,
                DateOfBirth="1381/03/03",
                PostImage= "images.jpg",
                Like="پسندیدن",
                Comment="نظردادن",
                Share="به اشتراک گذاشتن"
                
            };
            #endregion
            informationCustomerProfiles.Add(mohammadSallamat);
            #region Object Mapping Customer2            

            InformationCustomerProfile saeedSallamat = new InformationCustomerProfile()
            {
                profileImage= "Tailor.Man_.Situation.Sewing.Photos.jpg",
                Name= "SaeedSallamat",
                JobPosition = "خیاط",
                Age = 30,
                DateOfBirth = "1373/03/13",
                PostImage = "2276.jpg",
                Like = "پسندیدن",
                Comment = "نظردادن",
                Share = "به اشتراک گذاشتن"

            };
            #endregion
            informationCustomerProfiles.Add(saeedSallamat);

            _context.informationCustomerProfiles.AddRange(informationCustomerProfiles);
            _context.SaveChanges();

            return View(informationCustomerProfiles);
        }
    }
}
