using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Artikulo.Models.ViewModel
{
    public class UserModel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public int ProfileID { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string AccountImage { get; set; }

        // [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$", ErrorMessage = "Login ID should be a valid email address.")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "*")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one digit, and one special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public class UsersModel
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserLoginModel
    {
        [Key]
        public int SYSUserID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
