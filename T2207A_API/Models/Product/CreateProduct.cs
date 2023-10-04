using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace T2207A_API.Models.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = "acscdsc")]
        [MinLength(1)]
        [MaxLength(255)]
        public string name { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string thumbnail { get; set; }
        [Required]
        public int qty { get; set; }
        [Required]
        public int category_id { get; set; }
    }
}