using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data;

public class ClinicDbContext : DbContext
{
    public DbSet<PatientModel> Patient { get; set; }
    public DbSet<DoctorModel> Doctor { get; set; }
    public DbSet<MedCardModel> MedCard { get; set; }
    public DbSet<AppointmentModel> Appointment { get; set; }
    public DbSet<MedRecordModel> Med_record { get; set; }
    public DbSet<DiagnosisModel> Diagnosis { get; set; }
    public DbSet<SpecializationModel> Specialization { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DISPROPELE\\CLINICSQLSERVER,63453;Database=ClinicProDB;Trusted_Connection=True;User ID=dispropele;Password=masterkey;TrustServerCertificate=True;");
    }

    public bool TestConnection()
    {
        try
        {
            Database.OpenConnection();
            Database.CloseConnection();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("fail: " + e.Message);
            return false;
        }
    }
    
}