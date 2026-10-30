// Tệp: Controllers/HomeController.cs
// Mô tả: Controller cho các trang công khai như trang chủ, chi tiết chuyên khoa và trang lỗi.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Data;
using ClinicManagement.Models;
using System.Diagnostics;

namespace ClinicManagement.Controllers
{
    /// <summary>
    /// Controller để phục vụ trang chủ và các trang công khai khác như chi tiết chuyên khoa.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm dựng, DI inject ApplicationDbContext.
        /// </summary>
        /// <param name="context">DbContext của ứng dụng</param>
        public HomeController(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Trang chủ: tính các thống kê tổng quan và đưa ra danh sách chuyên khoa, bác sĩ nổi bật.
        /// </summary>
        /// <returns>View trang chủ</returns>
        public IActionResult Index()
        {
            ViewBag.TotalDoctors = _context.Doctors.Count();
            ViewBag.TotalPatients = _context.Users.Count(u => u.Role == "Patient");
            ViewBag.TotalAppointments = _context.Appointments.Count();
            ViewBag.TotalSpecialties = _context.Specialties.Count();
            ViewBag.Specialties = _context.Specialties.Include(s => s.Doctors).ToList();
            ViewBag.FeaturedDoctors = _context.Doctors
                .Include(d => d.User).Include(d => d.Specialty)
                .OrderBy(d => d.DoctorId).Take(6).ToList();
            return View();
        }

        /// <summary>
        /// Hiển thị chi tiết một chuyên khoa và danh sách bác sĩ thuộc chuyên khoa đó.
        /// </summary>
        /// <param name="id">Id chuyên khoa</param>
        /// <returns>View chi tiết chuyên khoa hoặc NotFound()</returns>
        public IActionResult SpecialtyDetail(int id)
        {
            var specialty = _context.Specialties.Include(s => s.Doctors).FirstOrDefault(s => s.SpecialtyId == id);
            if (specialty == null) return NotFound();
            var doctors = _context.Doctors.Include(d => d.User).Include(d => d.Specialty)
                .Where(d => d.SpecialtyId == id).ToList();
            ViewBag.Specialty = specialty;
            ViewBag.Doctors = doctors;
            return View();
        }

        /// <summary>
        /// Trang lỗi: trả về view lỗi với ErrorViewModel chứa RequestId.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
