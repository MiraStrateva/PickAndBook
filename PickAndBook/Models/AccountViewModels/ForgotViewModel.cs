using System.ComponentModel.DataAnnotations;

namespace PickAndBook.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}