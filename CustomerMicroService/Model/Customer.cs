using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMicroService.Model
{
    public enum Gender
    {
        Male,Female
    }
    public class Customer
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int CustomerId { get; set; }

        [Required]
        //[RegularExpression(@"^[A-Za-z]$", ErrorMessage = "Invalid Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNo { get; set; }
        
        [Required]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage ="Invalid PIN Number")]
        public int ZIPCode { get; set; }

        [Required]
        public string Address { get; set; }


        public class DateMinimumAgeAttribute : ValidationAttribute
        {
            public DateMinimumAgeAttribute(int minimumAge)
            {
                MinimumAge = minimumAge;
                ErrorMessage = "{0} must be someone at least {1} years of age";
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if ((value != null && DateTime.TryParse(value.ToString(), out date)))
                {
                    return date.AddYears(MinimumAge) < DateTime.Now;
                }

                return false;
            }

            public override string FormatErrorMessage(string name)
            {
                return string.Format(ErrorMessageString, name, MinimumAge);
            }

            public int MinimumAge { get; }
        }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required]
        [DateMinimumAge(18, ErrorMessage = "{0} must be someone at least {1} years of age")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required]
        public Gender gender { get; set; }

        [Required(ErrorMessage = "PAN Number is required")]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        public string PAN { get; set; }


        

    }
}
