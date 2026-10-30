using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class BookAppointmentViewModel
    {
        public int SpecialtyId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn bác sĩ")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày khám")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ khám")]
        public string AppointmentTime { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lý do khám")]
        [StringLength(500)]
        public string Reason { get; set; }

        // For display
        public List<Specialty>? Specialties { get; set; }
        public List<Doctor>? Doctors { get; set; }
        public List<string>? AvailableTimeSlots { get; set; }
    }
}
