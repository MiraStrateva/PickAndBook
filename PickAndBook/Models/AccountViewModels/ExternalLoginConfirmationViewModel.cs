using System.ComponentModel.DataAnnotations;

namespace PickAndBook.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}