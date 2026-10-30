using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class CreatePatientViewModel
    {
        [Required(ErrorMessage = "Ten dang nhap la bat buoc")]
        [StringLength(50)]
        [Display(Name = "Ten dang nhap")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mat khau la bat buoc")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mat khau phai co it nhat 6 ky tu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mat khau")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Ho ten la bat buoc")]
        [StringLength(100)]
        [Display(Name = "Ho va ten")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngay sinh la bat buoc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngay sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gioi tinh la bat buoc")]
        [Display(Name = "Gioi tinh")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "So dien thoai la bat buoc")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "So dien thoai phai co 10 chu so")]
        [Display(Name = "So dien thoai")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email la bat buoc")]
        [EmailAddress(ErrorMessage = "Email khong hop le")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Dia chi")]
        public string? Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Lien he khan cap")]
        public string? EmergencyContact { get; set; }

        [Display(Name = "Tien su benh")]
        public string? MedicalHistory { get; set; }
    }
}
