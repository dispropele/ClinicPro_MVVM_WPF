
namespace ClinicPro_MVVM_WPF.Model
{
    public class PatientModel
    {

        public int patientId { get; set; }
        
        public string polisNumber { get; set; } = string.Empty;
        
        public string firstName { get; set; } = string.Empty;
        
        public string lastName { get; set; } = string.Empty;
        
        public string? patronymic { get; set; }

        public DateTime? dateOfBirth { get; set; }
        
        public char gender { get; set; } // "M" или "F"
        
        public string? phoneNumber { get; set; }

        public string? email { get; set; }
        
        public string hashPassword { get; set; }  = string.Empty;
        
        
    }
}
