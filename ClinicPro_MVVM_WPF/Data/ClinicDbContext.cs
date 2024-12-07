using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data;

public class ClinicDbContext : DbContext
{
    public DbSet<PatientModel> Patient { get; set; }
    public DbSet<DoctorModel> Doctor { get; set; }
    public DbSet<MedCardModel> MedCard { get; set; }
    public DbSet<AppointmentModel> Appointment { get; set; }
    public DbSet<UserModel> User { get; set; }
    public DbSet<MedRecordModel> Med_Record { get; set; }
    public DbSet<DiagnosisModel> Diagnosis { get; set; }
    public DbSet<SpecializationModel> Specialization { get; set; }

    public ClinicDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   
        //Server=192.168.1.243\STUD_DB;Database=RMP_ClinicPro;Trusted_Connection=True; Integrated Security = True; TrustServerCertificate=True
        //Server=DISPROPELE\CLINICSQLSERVER,63680;Database=ClinicProDB;Trusted_Connection=True;User ID=dispropele;Password=masterkey;TrustServerCertificate=True;
        optionsBuilder.UseSqlServer("Server=192.168.1.243\\STUD_DB;Database=RMP_ClinicPro;Trusted_Connection=True; Integrated Security = True; TrustServerCertificate=True");
    }

    
    
}