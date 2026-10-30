using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Database.Migrate() is called in Program.cs before this runs
            if (context.Users.Any())
            {
                return;
            }

            var specialties = new[]
            {
                new Specialty { SpecialtyName = "Noi khoa", Description = "Chuyen khoa noi tong quat" },
                new Specialty { SpecialtyName = "Nhi khoa", Description = "Chuyen khoa tre em" },
                new Specialty { SpecialtyName = "Da lieu", Description = "Chuyen khoa da lieu" },
                new Specialty { SpecialtyName = "Tim mach", Description = "Chuyen khoa tim mach" },
                new Specialty { SpecialtyName = "Tai Mui Hong", Description = "Chuyen khoa tai mui hong" }
            };
            context.Specialties.AddRange(specialties);
            context.SaveChanges();

            var admin = new User
            {
                Username = "admin",
                Password = "admin123",
                FullName = "Quan tri vien",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Nam",
                PhoneNumber = "0900000000",
                Email = "admin@clinic.com",
                Role = "Admin",
                CreatedAt = DateTime.Now
            };
            context.Users.Add(admin);

            var doctorUsers = new[]
            {
                new User { Username = "bacsi01", Password = "123456", FullName = "BS. Nguyen Van An", DateOfBirth = new DateTime(1985, 3, 15), Gender = "Nam", PhoneNumber = "0901111111", Email = "nvan@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi02", Password = "123456", FullName = "ThS. Tran Thi Binh", DateOfBirth = new DateTime(1987, 5, 20), Gender = "Nu", PhoneNumber = "0902222222", Email = "ttbinh@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi03", Password = "123456", FullName = "TS. Le Hoang Cuong", DateOfBirth = new DateTime(1980, 7, 10), Gender = "Nam", PhoneNumber = "0903333333", Email = "lhcuong@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi04", Password = "123456", FullName = "BS. Pham Thi Dung", DateOfBirth = new DateTime(1988, 9, 25), Gender = "Nu", PhoneNumber = "0904444444", Email = "ptdung@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi05", Password = "123456", FullName = "ThS. Hoang Van Em", DateOfBirth = new DateTime(1986, 11, 30), Gender = "Nam", PhoneNumber = "0905555555", Email = "hvem@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi06", Password = "123456", FullName = "TS. Vu Thi Phuong", DateOfBirth = new DateTime(1982, 2, 14), Gender = "Nu", PhoneNumber = "0906666666", Email = "vtphuong@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi07", Password = "123456", FullName = "BS. Do Minh Giang", DateOfBirth = new DateTime(1989, 4, 18), Gender = "Nam", PhoneNumber = "0907777777", Email = "dmgiang@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi08", Password = "123456", FullName = "ThS. Bui Thi Hoa", DateOfBirth = new DateTime(1984, 6, 22), Gender = "Nu", PhoneNumber = "0908888888", Email = "bthoa@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi09", Password = "123456", FullName = "TS. Ngo Van Ich", DateOfBirth = new DateTime(1981, 8, 8), Gender = "Nam", PhoneNumber = "0909999999", Email = "nvich@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now },
                new User { Username = "bacsi10", Password = "123456", FullName = "BS. Dinh Thi Kim", DateOfBirth = new DateTime(1990, 10, 12), Gender = "Nu", PhoneNumber = "0901010101", Email = "dtkim@clinic.com", Role = "Doctor", CreatedAt = DateTime.Now }
            };
            context.Users.AddRange(doctorUsers);
            context.SaveChanges();

            var doctorRecords = new[]
            {
                new Doctor { UserId = doctorUsers[0].UserId, SpecialtyId = specialties[0].SpecialtyId, Degree = "BS", Experience = 10, ExaminationFee = 200000 },
                new Doctor { UserId = doctorUsers[1].UserId, SpecialtyId = specialties[1].SpecialtyId, Degree = "ThS", Experience = 8, ExaminationFee = 250000 },
                new Doctor { UserId = doctorUsers[2].UserId, SpecialtyId = specialties[2].SpecialtyId, Degree = "TS", Experience = 15, ExaminationFee = 350000 },
                new Doctor { UserId = doctorUsers[3].UserId, SpecialtyId = specialties[3].SpecialtyId, Degree = "BS", Experience = 7, ExaminationFee = 220000 },
                new Doctor { UserId = doctorUsers[4].UserId, SpecialtyId = specialties[4].SpecialtyId, Degree = "ThS", Experience = 9, ExaminationFee = 280000 },
                new Doctor { UserId = doctorUsers[5].UserId, SpecialtyId = specialties[0].SpecialtyId, Degree = "TS", Experience = 12, ExaminationFee = 320000 },
                new Doctor { UserId = doctorUsers[6].UserId, SpecialtyId = specialties[1].SpecialtyId, Degree = "BS", Experience = 6, ExaminationFee = 210000 },
                new Doctor { UserId = doctorUsers[7].UserId, SpecialtyId = specialties[2].SpecialtyId, Degree = "ThS", Experience = 11, ExaminationFee = 300000 },
                new Doctor { UserId = doctorUsers[8].UserId, SpecialtyId = specialties[3].SpecialtyId, Degree = "TS", Experience = 14, ExaminationFee = 340000 },
                new Doctor { UserId = doctorUsers[9].UserId, SpecialtyId = specialties[4].SpecialtyId, Degree = "BS", Experience = 5, ExaminationFee = 200000 }
            };
            context.Doctors.AddRange(doctorRecords);
            context.SaveChanges();

            var random = new Random();
            var patients = new List<User>();
            for (int i = 1; i <= 50; i++)
            {
                patients.Add(new User
                {
                    Username = $"patient{i:D2}",
                    Password = "123456",
                    FullName = $"Benh nhan {i}",
                    DateOfBirth = new DateTime(1970 + random.Next(40), random.Next(1, 13), random.Next(1, 29)),
                    Gender = i % 2 == 0 ? "Nu" : "Nam",
                    PhoneNumber = $"09{random.Next(10000000, 99999999)}",
                    Email = $"patient{i}@email.com",
                    Role = "Patient",
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }
            context.Users.AddRange(patients);
            context.SaveChanges();

            var appointments = new List<Appointment>();
            var timeSlots = new[] { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00", "17:00" };

            var reasonsDaDat = new[] { "Đau đầu chóng mặt", "Kiểm tra sức khỏe định kỳ", "Ho kéo dài", "Đau bụng", "Dị ứng da", "Khó thở", "Mất ngủ", "Đau ngực", "Sốt cao", "Đau lưng" };
            var reasonsDaKham = new[] { "Khám tổng quát", "Đau họng viêm amidan", "Viêm xoang mãn tính", "Dị ứng theo mùa", "Đau mỏi cơ xương", "Rối loạn tiêu hóa", "Tăng huyết áp", "Đái tháo đường kiểm soát", "Viêm da tiếp xúc", "Tim đập nhanh bất thường" };
            var symptomsList = new[] { "Đau đầu dữ dội, chóng mặt, buồn nôn", "Ho khan kéo dài > 2 tuần, có đờm", "Sốt 38-39°C, mệt mỏi toàn thân", "Đau bụng vùng thượng vị, ăn kém", "Nổi mẩn đỏ, ngứa toàn thân", "Khó thở khi gắng sức, hồi hộp" };
            var diagnosisList = new[] { "Viêm xoang cấp", "Viêm phế quản mạn", "Tăng huyết áp độ 1", "Viêm dạ dày HP(+)", "Viêm da dị ứng", "Rối loạn nhịp tim lành tính" };
            var adviceList = new[] { "Uống thuốc đủ liều theo đơn. Nghỉ ngơi đầy đủ, tránh làm việc nặng. Tái khám sau 1 tuần.", "Thuốc kháng sinh 7 ngày, uống sau ăn. Uống nhiều nước ấm. Hạn chế đồ lạnh, cay.", "Đo huyết áp hàng ngày. Giảm muối trong ăn uống. Tập thể dục nhẹ 30 phút/ngày.", "Tránh ăn đồ chua, cay, rượu bia. Uống thuốc bảo vệ dạ dày trước ăn. Tái khám sau 2 tuần." };

            // Tạo lịch hẹn "Đã đặt" (tương lai) - mỗi bác sĩ có 3-5 lịch chờ khám
            foreach (var doc in doctorRecords)
            {
                int waitingCount = random.Next(3, 6);
                var usedSlots = new HashSet<(DateTime, string)>();
                for (int i = 0; i < waitingCount; i++)
                {
                    var date = DateTime.Today.AddDays(random.Next(1, 14));
                    string slot;
                    int tries = 0;
                    do { slot = timeSlots[random.Next(timeSlots.Length)]; tries++; } while (usedSlots.Contains((date, slot)) && tries < 20);
                    usedSlots.Add((date, slot));
                    appointments.Add(new Appointment
                    {
                        PatientId = patients[random.Next(patients.Count)].UserId,
                        DoctorId = doc.DoctorId,
                        AppointmentDate = date,
                        AppointmentTime = slot,
                        Reason = reasonsDaDat[random.Next(reasonsDaDat.Length)],
                        Status = "Đã đặt"
                    });
                }
            }

            // Tạo lịch hẹn "Đã khám" (quá khứ) - mỗi bác sĩ có 8-12 lịch đã khám
            foreach (var doc in doctorRecords)
            {
                int doneCount = random.Next(8, 13);
                for (int i = 0; i < doneCount; i++)
                {
                    var date = DateTime.Today.AddDays(-random.Next(1, 60));
                    appointments.Add(new Appointment
                    {
                        PatientId = patients[random.Next(patients.Count)].UserId,
                        DoctorId = doc.DoctorId,
                        AppointmentDate = date,
                        AppointmentTime = timeSlots[random.Next(timeSlots.Length)],
                        Reason = reasonsDaKham[random.Next(reasonsDaKham.Length)],
                        Status = "Đã khám"
                    });
                }
            }

            // Tạo lịch hẹn "Đã hủy" - mỗi bác sĩ 2-4 lịch hủy
            foreach (var doc in doctorRecords)
            {
                int cancelCount = random.Next(2, 5);
                for (int i = 0; i < cancelCount; i++)
                {
                    var date = DateTime.Today.AddDays(-random.Next(5, 90));
                    appointments.Add(new Appointment
                    {
                        PatientId = patients[random.Next(patients.Count)].UserId,
                        DoctorId = doc.DoctorId,
                        AppointmentDate = date,
                        AppointmentTime = timeSlots[random.Next(timeSlots.Length)],
                        Reason = $"Lý do khám {random.Next(100)}",
                        Status = "Đã hủy"
                    });
                }
            }

            context.Appointments.AddRange(appointments);
            context.SaveChanges();

            var examinedAppointments = appointments.Where(a => a.Status == "Đã khám").ToList();
            var medicalRecords = new List<MedicalRecord>();
            foreach (var appointment in examinedAppointments)
            {
                medicalRecords.Add(new MedicalRecord
                {
                    AppointmentId = appointment.AppointmentId,
                    Symptoms = symptomsList[random.Next(symptomsList.Length)],
                    Diagnosis = diagnosisList[random.Next(diagnosisList.Length)],
                    Advice = adviceList[random.Next(adviceList.Length)],
                    ExaminationDate = appointment.AppointmentDate
                });
            }
            context.MedicalRecords.AddRange(medicalRecords);
            context.SaveChanges();
        }
    }
}
