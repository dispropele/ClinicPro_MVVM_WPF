using Microsoft.EntityFrameworkCore;
using ClinicPro_MVVM_WPF.Model.Patient;
using ClinicPro_MVVM_WPF.Model.Doctor;

namespace ClinicPro_MVVM_WPF.Model
{
    public class ApplicationDbContext : DbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PatientModel> Patients { get; set; } = null!;
        public DbSet<DoctorModel> Doctors { get; set; } = null!;
        
        //остальные модели

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DISPROPELE\CLINICSQLSERVER;Database = ClinicProDB;Trusted_Connection=True");
        }

    }
}
