using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class ExaminationViewModel
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Triệu chứng là bắt buộc")]
        [StringLength(1000)]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Chẩn đoán là bắt buộc")]
        [StringLength(1000)]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Lời dặn là bắt buộc")]
        [StringLength(1000)]
        public string Advice { get; set; }

        // For display
        public Appointment? Appointment { get; set; }
    }
}
