using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ClinicManagement.Data;

#nullable disable

namespace ClinicManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicManagement.Models.Appointment", b =>
                {
                    b.Property<int>("AppointmentId").ValueGeneratedOnAdd().HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));
                    b.Property<DateTime>("AppointmentDate").HasColumnType("datetime2");
                    b.Property<string>("AppointmentTime").IsRequired().HasColumnType("nvarchar(450)");
                    b.Property<int>("DoctorId").HasColumnType("int");
                    b.Property<int>("PatientId").HasColumnType("int");
                    b.Property<string>("Reason").IsRequired().HasMaxLength(500).HasColumnType("nvarchar(500)");
                    b.Property<string>("Status").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
                    b.HasKey("AppointmentId");
                    b.HasIndex("DoctorId", "AppointmentDate", "AppointmentTime");
                    b.HasIndex("PatientId");
                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("ClinicManagement.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorId").ValueGeneratedOnAdd().HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoctorId"));
                    b.Property<string>("Degree").IsRequired().HasMaxLength(10).HasColumnType("nvarchar(10)");
                    b.Property<decimal>("ExaminationFee").HasColumnType("decimal(18,2)");
                    b.Property<int>("Experience").HasColumnType("int");
                    b.Property<int>("SpecialtyId").HasColumnType("int");
                    b.Property<int>("UserId").HasColumnType("int");
                    b.Property<string>("PhotoUrl").HasMaxLength(500).HasColumnType("nvarchar(500)");
                    b.Property<string>("Bio").HasMaxLength(1000).HasColumnType("nvarchar(1000)");
                    b.HasKey("DoctorId");
                    b.HasIndex("SpecialtyId");
                    b.HasIndex("UserId").IsUnique();
                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("ClinicManagement.Models.MedicalRecord", b =>
                {
                    b.Property<int>("RecordId").ValueGeneratedOnAdd().HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordId"));
                    b.Property<string>("Advice").IsRequired().HasMaxLength(1000).HasColumnType("nvarchar(1000)");
                    b.Property<int>("AppointmentId").HasColumnType("int");
                    b.Property<string>("Diagnosis").IsRequired().HasMaxLength(1000).HasColumnType("nvarchar(1000)");
                    b.Property<DateTime>("ExaminationDate").HasColumnType("datetime2");
                    b.Property<string>("Symptoms").IsRequired().HasMaxLength(1000).HasColumnType("nvarchar(1000)");
                    b.HasKey("RecordId");
                    b.HasIndex("AppointmentId").IsUnique();
                    b.ToTable("MedicalRecords");
                });

            modelBuilder.Entity("ClinicManagement.Models.Specialty", b =>
                {
                    b.Property<int>("SpecialtyId").ValueGeneratedOnAdd().HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecialtyId"));
                    b.Property<string>("Description").HasMaxLength(500).HasColumnType("nvarchar(500)");
                    b.Property<string>("SpecialtyName").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
                    b.HasKey("SpecialtyId");
                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("ClinicManagement.Models.User", b =>
                {
                    b.Property<int>("UserId").ValueGeneratedOnAdd().HasColumnType("int");
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));
                    b.Property<DateTime>("DateOfBirth").HasColumnType("datetime2");
                    b.Property<string>("Email").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
                    b.Property<string>("FullName").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
                    b.Property<string>("Gender").IsRequired().HasMaxLength(10).HasColumnType("nvarchar(10)");
                    b.Property<string>("Password").IsRequired().HasMaxLength(255).HasColumnType("nvarchar(255)");
                    b.Property<string>("PhoneNumber").IsRequired().HasMaxLength(10).HasColumnType("nvarchar(10)");
                    b.Property<string>("Role").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
                    b.Property<string>("Username").IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
                    b.Property<string>("Address").HasMaxLength(200).HasColumnType("nvarchar(200)");
                    b.Property<string>("EmergencyContact").HasMaxLength(100).HasColumnType("nvarchar(100)");
                    b.Property<string>("MedicalHistory").HasColumnType("nvarchar(max)");
                    b.Property<DateTime?>("CreatedAt").HasColumnType("datetime2");
                    b.HasKey("UserId");
                    b.HasIndex("Username").IsUnique();
                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClinicManagement.Models.Appointment", b =>
                {
                    b.HasOne("ClinicManagement.Models.Doctor", "Doctor")
                        .WithMany("Appointments").HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();
                    b.HasOne("ClinicManagement.Models.User", "Patient")
                        .WithMany("Appointments").HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();
                    b.Navigation("Doctor");
                    b.Navigation("Patient");
                });

            modelBuilder.Entity("ClinicManagement.Models.Doctor", b =>
                {
                    b.HasOne("ClinicManagement.Models.Specialty", "Specialty")
                        .WithMany("Doctors").HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();
                    b.HasOne("ClinicManagement.Models.User", "User")
                        .WithOne("Doctor")
                        .HasForeignKey("ClinicManagement.Models.Doctor", "UserId")
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();
                    b.Navigation("Specialty");
                    b.Navigation("User");
                });

            modelBuilder.Entity("ClinicManagement.Models.MedicalRecord", b =>
                {
                    b.HasOne("ClinicManagement.Models.Appointment", "Appointment")
                        .WithOne("MedicalRecord")
                        .HasForeignKey("ClinicManagement.Models.MedicalRecord", "AppointmentId")
                        .OnDelete(DeleteBehavior.Restrict).IsRequired();
                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("ClinicManagement.Models.Appointment", b => { b.Navigation("MedicalRecord"); });
            modelBuilder.Entity("ClinicManagement.Models.Doctor", b => { b.Navigation("Appointments"); });
            modelBuilder.Entity("ClinicManagement.Models.Specialty", b => { b.Navigation("Doctors"); });
            modelBuilder.Entity("ClinicManagement.Models.User", b =>
                {
                    b.Navigation("Appointments");
                    b.Navigation("Doctor");
                });
#pragma warning restore 612, 618
        }
    }
}
