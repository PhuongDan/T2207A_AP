using System.ComponentModel.DataAnnotations;


namespace T2207A_API.Models.Category
{
    public class EditCategory
    {
        [Required]
        public int id { get; set; }


        [Required(ErrorMessage = "acscdsc")]
        [MinLength(1)]
        [MaxLength(255)]
        public string name { get; set; }
    }
}
