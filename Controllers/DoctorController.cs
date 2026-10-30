// Tệp: Controllers/DoctorController.cs
// Mô tả: Controller dành cho vai trò Doctor, bao gồm các action để bác sĩ
//         quản lý lịch khám, lưu kết quả khám, quản lý bệnh nhân và hồ sơ.
// Ghi chú: Controller sử dụng phân quyền theo vai trò và đọc id người dùng từ claims.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Data;
using ClinicManagement.Models;
using ClinicManagement.Models.ViewModels;
using System.Security.Claims;

namespace ClinicManagement.Controllers
{
    [Authorize(Roles = "Doctor")]
    /// <summary>
    /// Controller cho chức năng bác sĩ: xem danh sách lịch hẹn trong ngày,
    /// quản lý lịch hẹn, khám và lưu hồ sơ bệnh án, tạo bệnh nhân mới và cập nhật hồ sơ bác sĩ.
    /// </summary>
    public class DoctorController : Controller
    {
        // DbContext và môi trường web để lưu file (ảnh) nếu cần
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Hàm dựng controller. DbContext và IWebHostEnvironment được tiêm bởi DI.
        /// </summary>
        /// <param name="context">DbContext của ứng dụng</param>
        /// <param name="env">Môi trường host để xử lý file tĩnh</param>
        public DoctorController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Lấy id bác sĩ từ user id trong claim. Trả về 0 nếu không tìm thấy bác sĩ tương ứng.
        /// </summary>
        /// <returns>DoctorId hoặc 0 nếu không tồn tại</returns>
        private int GetDoctorId()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return _context.Doctors.FirstOrDefault(d => d.UserId == userId)?.DoctorId ?? 0;
        }

        /// <summary>
        /// Lấy thực thể Doctor của người dùng hiện tại, kèm dữ liệu User và Specialty.
        /// </summary>
        /// <returns>Doctor hoặc null nếu không tìm thấy</returns>
        private Doctor? GetCurrentDoctor()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return _context.Doctors.Include(d => d.User).Include(d => d.Specialty).FirstOrDefault(d => d.UserId == userId);
        }

        /// <summary>
        /// Trang tổng quan cho bác sĩ: hiển thị các lịch hẹn trong ngày, thông tin
        /// thống kê như số bệnh chờ, đã khám, tổng trong tháng và tổng tất cả.
        /// </summary>
        /// <returns>View với danh sách lịch hẹn hôm nay</returns>
        public IActionResult Index()
        {
            var doctor = GetCurrentDoctor();
            if (doctor == null) return NotFound();

            var todayAppointments = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctor.DoctorId && a.AppointmentDate == DateTime.Today)
                .OrderBy(a => a.AppointmentTime).ToList();

            ViewBag.Doctor = doctor;
            ViewBag.WaitingCount = todayAppointments.Count(a => a.Status == "Đã đặt" || a.Status == "Da dat");
            ViewBag.DoneCount = todayAppointments.Count(a => a.Status == "Đã khám" || a.Status == "Da kham");
            ViewBag.TotalThisMonth = _context.Appointments.Count(a => a.DoctorId == doctor.DoctorId && a.AppointmentDate.Month == DateTime.Today.Month && a.AppointmentDate.Year == DateTime.Today.Year);
            ViewBag.TotalAll = _context.Appointments.Count(a => a.DoctorId == doctor.DoctorId);
            return View(todayAppointments);
        }

        /// <summary>
        /// Liệt kê lịch hẹn của bác sĩ, có thể lọc theo trạng thái.
        /// </summary>
        /// <param name="status">Trạng thái (tùy chọn) để lọc ví dụ: "Đã đặt", "Đã khám"</param>
        /// <returns>View chứa danh sách lịch hẹn</returns>
        public IActionResult MyAppointments(string status = "")
        {
            var doctorId = GetDoctorId();
            var query = _context.Appointments.Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId).AsQueryable();
            if (!string.IsNullOrEmpty(status)) query = query.Where(a => a.Status == status);
            var appointments = query.OrderByDescending(a => a.AppointmentDate).ThenByDescending(a => a.AppointmentTime).ToList();
            ViewBag.Status = status;
            return View(appointments);
        }

        /// <summary>
        /// GET: Mở form khám cho một lịch hẹn cụ thể (nếu lịch ở trạng thái hợp lệ).
        /// </summary>
        /// <param name="id">Id của Appointment</param>
        /// <returns>View với ExaminationViewModel hoặc NotFound()</returns>
        public IActionResult Examine(int id)
        {
            var appointment = _context.Appointments.Include(a => a.Patient).FirstOrDefault(a => a.AppointmentId == id);
            if (appointment == null || (appointment.Status != "Đã đặt" && appointment.Status != "Da dat")) return NotFound();
            var model = new ExaminationViewModel { AppointmentId = id, Appointment = appointment };
            return View(model);
        }

        /// <summary>
        /// POST: Lưu kết quả khám, tạo MedicalRecord và cập nhật trạng thái lịch thành "Đã khám".
        /// </summary>
        /// <param name="model">Dữ liệu khám từ form</param>
        /// <returns>Chuyển hướng về Index khi thành công hoặc trả về view với lỗi</returns>
        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult Examine(ExaminationViewModel model)
        {
            ModelState.Remove("Appointment");
            if (ModelState.IsValid)
            {
                var appointment = _context.Appointments.Find(model.AppointmentId);
                if (appointment != null)
                {
                    _context.MedicalRecords.Add(new MedicalRecord { AppointmentId = model.AppointmentId, Symptoms = model.Symptoms, Diagnosis = model.Diagnosis, Advice = model.Advice, ExaminationDate = DateTime.Today });
                    appointment.Status = "Đã khám";
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Lưu kết quả khám thành công!";
                    return RedirectToAction("Index");
                }
            }
            model.Appointment = _context.Appointments.Include(a => a.Patient).FirstOrDefault(a => a.AppointmentId == model.AppointmentId);
            return View(model);
        }

        /// <summary>
        /// Hiển thị lịch sử khám (MedicalRecords) của một bệnh nhân cụ thể.
        /// </summary>
        /// <param name="patientId">Id người bệnh</param>
        /// <returns>View chứa danh sách MedicalRecord</returns>
        public IActionResult PatientHistory(int patientId)
        {
            var records = _context.MedicalRecords
                .Include(m => m.Appointment).ThenInclude(a => a.Patient)
                .Where(m => m.Appointment.PatientId == patientId)
                .OrderByDescending(m => m.ExaminationDate).ToList();
            ViewBag.Patient = _context.Users.Find(patientId);
            return View(records);
        }

        // ==================== THÊM BỆNH NHÂN ====================
        /// <summary>
        /// GET: Hiển thị form tạo bệnh nhân mới (dành cho bác sĩ).
        /// </summary>
        /// <returns>View form tạo bệnh nhân</returns>
        [HttpGet]
        public IActionResult CreatePatient()
        {
            return View();
        }

        /// <summary>
        /// POST: Tạo người dùng mới với vai trò Patient. Kiểm tra trùng username/email.
        /// </summary>
        /// <param name="model">Dữ liệu người bệnh mới</param>
        /// <returns>Chuyển hướng đến BookAppointment để đặt lịch cho bệnh nhân mới</returns>
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatient(CreatePatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                { ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại"); return View(model); }
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                { ModelState.AddModelError("Email", "Email đã được sử dụng"); return View(model); }

                var user = new User
                {
                    Username = model.Username, Password = model.Password,
                    FullName = model.FullName, DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender, PhoneNumber = model.PhoneNumber,
                    Email = model.Email, Address = model.Address,
                    EmergencyContact = model.EmergencyContact, MedicalHistory = model.MedicalHistory,
                    Role = "Patient", CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Thêm bệnh nhân '{user.FullName}' thành công!";
                TempData["NewPatientId"] = user.UserId;
                return RedirectToAction(nameof(BookAppointment), new { patientId = user.UserId });
            }
            return View(model);
        }

        // ==================== ĐẶT LỊCH HẸN ====================
        /// <summary>
        /// GET: Chuẩn bị form đặt lịch cho bác sĩ, bao gồm danh sách bệnh nhân,
        /// các khung giờ có thể chọn và các lịch trong ngày.
        /// </summary>
        /// <param name="patientId">Id bệnh nhân được chọn trước (tùy chọn)</param>
        /// <returns>View form đặt lịch</returns>
        [HttpGet]
        public IActionResult BookAppointment(int? patientId)
        {
            var doctorId = GetDoctorId();
            var doctor = GetCurrentDoctor();

            ViewBag.Patients = _context.Users
                .Where(u => u.Role == "Patient")
                .OrderBy(u => u.FullName).ToList();
            ViewBag.DoctorId = doctorId;
            ViewBag.DoctorName = doctor?.User?.FullName;
            ViewBag.TimeSlots = new[] { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00", "17:00" };
            ViewBag.SelectedPatientId = patientId;
            ViewBag.TodayAppointments = _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == DateTime.Today)
                .OrderBy(a => a.AppointmentTime).ToList();
            return View();
        }

        /// <summary>
        /// POST: Tạo lịch hẹn do bác sĩ đặt cho bệnh nhân. Kiểm tra dữ liệu đầu vào
        /// và xung đột thời gian trước khi lưu.
        /// </summary>
        /// <param name="patientId">Id bệnh nhân</param>
        /// <param name="appointmentDate">Ngày hẹn</param>
        /// <param name="appointmentTime">Giờ hẹn</param>
        /// <param name="reason">Lý do</param>
        /// <returns>Chuyển hướng đến Index hoặc trả về lỗi qua TempData</returns>
        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult BookAppointment(int patientId, DateTime appointmentDate, string appointmentTime, string reason)
        {
            var doctorId = GetDoctorId();

            if (patientId == 0 || appointmentDate == default || string.IsNullOrEmpty(appointmentTime) || string.IsNullOrEmpty(reason))
            { TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin!"; return RedirectToAction(nameof(BookAppointment)); }

            if (appointmentDate.Date < DateTime.Today)
            { TempData["ErrorMessage"] = "Ngày hẹn không được là ngày trong quá khứ!"; return RedirectToAction(nameof(BookAppointment)); }

            var conflict = _context.Appointments.Any(a =>
                a.DoctorId == doctorId && a.AppointmentDate.Date == appointmentDate.Date &&
                a.AppointmentTime == appointmentTime && a.Status != "Đã hủy");
            if (conflict)
            { TempData["ErrorMessage"] = "Khung giờ này đã có lịch! Vui lòng chọn giờ khác."; return RedirectToAction(nameof(BookAppointment)); }

            _context.Appointments.Add(new Appointment
            {
                PatientId = patientId, DoctorId = doctorId,
                AppointmentDate = appointmentDate.Date,
                AppointmentTime = appointmentTime,
                Reason = reason, Status = "Đã đặt"
            });
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Đặt lịch hẹn thành công!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// API AJAX: Trả về các khung giờ còn trống cho bác sĩ trong ngày được cung cấp.
        /// </summary>
        /// <param name="date">Ngày cần kiểm tra</param>
        /// <returns>Danh sách các khung giờ còn trống</returns>
        [HttpGet]
        public JsonResult GetAvailableTimeSlots(DateTime date)
        {
            var doctorId = GetDoctorId();
            var allSlots = new[] { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00", "17:00" };
            var booked = _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == date.Date && a.Status != "Đã hủy")
                .Select(a => a.AppointmentTime).ToList();
            return Json(allSlots.Except(booked).ToList());
        }

        /// <summary>
        /// GET: Hiển thị trang hồ sơ bác sĩ hiện tại.
        /// </summary>
        /// <returns>View với đối tượng Doctor</returns>
        public IActionResult Profile()
        {
            var doctor = GetCurrentDoctor();
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        /// <summary>
        /// POST: Cập nhật hồ sơ bác sĩ, bao gồm ảnh (lưu file vào wwwroot) và mật khẩu.
        /// Lưu ý: ví dụ này ghi thẳng mật khẩu; cần băm mật khẩu trong thực tế.
        /// </summary>
        /// <param name="fullName">Họ và tên</param>
        /// <param name="dateOfBirth">Ngày sinh</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="email">Email</param>
        /// <param name="bio">Tiểu sử/giới thiệu</param>
        /// <param name="newPassword">Mật khẩu mới (tùy chọn)</param>
        /// <param name="photo">Ảnh hồ sơ (tùy chọn)</param>
        /// <returns>Chuyển hướng về Profile sau khi lưu</returns>
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string fullName, DateTime dateOfBirth, string gender, string phoneNumber, string email, string? bio, string? newPassword, IFormFile? photo)
        {
            var doctor = GetCurrentDoctor();
            if (doctor == null) return NotFound();
            doctor.User.FullName = fullName;
            doctor.User.DateOfBirth = dateOfBirth;
            doctor.User.Gender = gender;
            doctor.User.PhoneNumber = phoneNumber;
            doctor.User.Email = email;
            doctor.Bio = bio;
            if (!string.IsNullOrEmpty(newPassword)) doctor.User.Password = newPassword;
            if (photo != null && photo.Length > 0)
            {
                var fileName = $"doctor_{doctor.UserId}_{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}";
                var savePath = Path.Combine(_env.WebRootPath, "img", "doctors", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                using var stream = new FileStream(savePath, FileMode.Create);
                await photo.CopyToAsync(stream);
                doctor.PhotoUrl = $"/img/doctors/{fileName}";
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }
    }
}
