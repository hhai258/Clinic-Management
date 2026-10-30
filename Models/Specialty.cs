// Tệp: Models/Specialty.cs
// Mô tả: Mô hình dữ liệu chuyên khoa, định danh các lĩnh vực chuyên môn của bác sĩ.

using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models
{
    /// <summary>
    /// Đại diện cho một chuyên khoa (Nội khoa, Nhi khoa, Da liễu, v.v.).
    /// Mỗi chuyên khoa có thể có nhiều bác sĩ.
    /// </summary>
    public class Specialty
    {
        /// <summary>Định danh duy nhất của chuyên khoa (khóa chính)</summary>
        [Key]
        public int SpecialtyId { get; set; }

        /// <summary>Tên chuyên khoa</summary>
        [Required(ErrorMessage = "Tên chuyên khoa là bắt buộc")]
        [StringLength(100)]
        public string SpecialtyName { get; set; }

        /// <summary>Mô tả chi tiết về chuyên khoa (tùy chọn)</summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>Thuộc tính điều hướng: danh sách các bác sĩ thuộc chuyên khoa này</summary>
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
