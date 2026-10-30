// Tệp: Controllers/AccountController.cs
// Mô tả: Controller xử lý xác thực người dùng: đăng nhập, đăng ký, đăng xuất và truy cập bị từ chối.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ClinicManagement.Data;
using ClinicManagement.Models;
using ClinicManagement.Models.ViewModels;

namespace ClinicManagement.Controllers
{
    /// <summary>
    /// Controller quản lý tài khoản: login, register, logout và trang access denied.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Hàm dựng, DB context được tiêm bởi DI.
        /// </summary>
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Hiển thị trang đăng nhập.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST: Xác thực người dùng theo username và password. Nếu hợp lệ sẽ tạo cookie
        /// authentication và chuyển hướng theo vai trò (Admin/Doctor/Patient).
        /// Lưu ý: mật khẩu hiện được so sánh trực tiếp; cần hash trong môi trường production.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            
            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.FullName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Chuyển hướng theo vai trò
            return user.Role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Doctor" => RedirectToAction("Index", "Doctor"),
                "Patient" => RedirectToAction("Index", "Patient"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        /// <summary>
        /// GET: Hiển thị trang đăng ký.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST: Tạo tài khoản mới với vai trò mặc định là Patient. Kiểm tra trùng username.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra username đã tồn tại
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Password = model.Password, // Trong production cần băm mật khẩu
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Role = "Patient" // Vai trò mặc định
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        /// <summary>
        /// POST: Đăng xuất người dùng và xoá cookie authentication.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Hiển thị trang Access Denied khi người dùng không có quyền truy cập.
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
