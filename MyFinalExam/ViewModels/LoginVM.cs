using System.ComponentModel.DataAnnotations;

namespace MyFinalExam.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Account")]
        [Required(ErrorMessage = "Please Enter Account To Login")]
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        public string Id { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Account To Login")]
        [DataType(DataType.Password)]
        public string passwordLogin { get; set; }

    }
}
