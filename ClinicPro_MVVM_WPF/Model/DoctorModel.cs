
namespace ClinicPro_MVVM_WPF.Model
{
    public class DoctorModel
    {

        public int doctorId { get; set; }
        
        public string firstName { get; set; } = string.Empty;

        public string lastName { get; set; } = string.Empty;
        
        public string? patronymic { get; set; }
        
        public string? officeNumber { get; set; }
        
        public char gender { get; set; }
        
        public string? email { get; set; }
        
        public string? phoneNumber { get; set; }
        
        public byte[]? photo { get; set; } // Для хранения изображения
        
        public string hashPassword { get; set; } = string.Empty;
        
        public virtual SpecializationModel specialization { get; set; }
    }
}
