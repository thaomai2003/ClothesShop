using System.ComponentModel.DataAnnotations;

namespace MyFinalExam.Areas.Admin.Models.ViewModels
{
    public class RegisterViewModel
    {
        [MaxLength(40, ErrorMessage = "Max length is 40 characters !")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Account")]
        public string Id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(50, ErrorMessage = "Max length is 50 characters !")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect Email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(60, ErrorMessage = "Max length is 60 characters !")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [MaxLength(24, ErrorMessage = "Max length is 24 characters !")]
        [RegularExpression(@"^0[9875]\d{8}$", ErrorMessage = "Incorrect phone format")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Sex")]
        public bool? Sex { get; set; } = true;
    }
}
