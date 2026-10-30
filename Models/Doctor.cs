// Tệp: Models/Doctor.cs
// Mô tả: Mô hình dữ liệu bác sĩ chứa thông tin khám, chuyên khoa và phí dịch vụ.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagement.Models
{
    /// <summary>
    /// Đại diện cho một bác sĩ trong hệ thống. Mỗi bác sĩ được liên kết với
    /// một tài khoản người dùng, một chuyên khoa, và nhiều cuộc hẹn.
    /// </summary>
    public class Doctor
    {
        /// <summary>Định danh duy nhất của bác sĩ (khóa chính)</summary>
        [Key]
        public int DoctorId { get; set; }

        /// <summary>Khóa ngoại liên kết đến User (tài khoản của bác sĩ)</summary>
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>Khóa ngoại liên kết đến Specialty (chuyên khoa)</summary>
        [Required]
        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; }

        /// <summary>Học vị (BS, ThS, TS, v.v.)</summary>
        [Required(ErrorMessage = "Học vị là bắt buộc")]
        [StringLength(10)]
        public string Degree { get; set; }

        /// <summary>Số năm kinh nghiệm làm việc</summary>
        [Required(ErrorMessage = "Kinh nghiệm là bắt buộc")]
        [Range(0, 50, ErrorMessage = "Kinh nghiệm phải từ 0 đến 50 năm")]
        public int Experience { get; set; }

        /// <summary>Phí khám bệnh (VNĐ)</summary>
        [Required(ErrorMessage = "Giá khám là bắt buộc")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá khám phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExaminationFee { get; set; }

        /// <summary>URL ảnh hồ sơ của bác sĩ (tùy chọn)</summary>
        [StringLength(500)]
        public string? PhotoUrl { get; set; }

        /// <summary>Tiểu sử/giới thiệu về bác sĩ (tùy chọn)</summary>
        [StringLength(1000)]
        public string? Bio { get; set; }

        /// <summary>Thuộc tính điều hướng: tài khoản người dùng của bác sĩ</summary>
        public User User { get; set; }

        /// <summary>Thuộc tính điều hướng: chuyên khoa của bác sĩ</summary>
        public Specialty Specialty { get; set; }

        /// <summary>Thuộc tính điều hướng: danh sách các lịch hẹn của bác sĩ</summary>
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
