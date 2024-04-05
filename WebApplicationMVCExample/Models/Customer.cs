using System.ComponentModel.DataAnnotations;

namespace WebApplicationMVCExample.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Customer name is required")]
        public string Name { get; set; } = null!;

        [Display(Name = "Address1")]
        [Required(ErrorMessage = "Address1 is required")]
        public string? Address1 { get; set; }

        [Display(Name = "Address2")]
        public string? Address2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; } = null!;

        [Display(Name = "Province or State")]
        [Required(ErrorMessage = "Province or State is required")]
        [StringLength(2, ErrorMessage = "Invalid state code")]
        public string? ProvinceOrState { get; set; } = null!;

        [Display(Name = "Zip or Postal Code")]
        [Required(ErrorMessage = "Zip or postal code is required")]
        [RegularExpression(@"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]{1}\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstv‌​xy]{1} *\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]{1}\d{1}$)", ErrorMessage = "Not a valid US zip or Canadian postal code.")]
        public string? ZipOrPostalCode { get; set; } = null!;

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }

        //[Required(ErrorMessage = "Contact last name is required")]
        public string? ContactLastName { get; set; }

        //[Required(ErrorMessage = "Contact first name is required")]
        public string? ContactFirstName { get; set; }

        [Display(Name = "Contact Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
