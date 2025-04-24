using System.ComponentModel.DataAnnotations;

namespace PersianResumeBuilder.DTOs
{
    public class OrderDTO
    {
        [Display(Name ="نام مشتری")]
        public string FullName { get; set; }

        [Display(Name = "نام محصولات خریداری شده ")]
        public string Item { get; set; }

        [Display(Name = "(Toman)مبلغ کل سفارش")]
        public decimal Price { get; set; }
    }
}
