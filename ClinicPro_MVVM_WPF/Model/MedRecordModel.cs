
namespace ClinicPro_MVVM_WPF.Model
{
    public class MedRecordModel
    {
        public int recordId { get; set; }
        
        public int medcardId { get; set; }
        
        public int doctorId { get; set; }
        
        public DateTime dateTime { get; set; }
        
        public string complaints { get; set; }
        
        public string examination { get; set; }
        
        public string recommendation { get; set; }
        
        public string treatment { get; set; }

        public virtual MedCardModel medCard { get; set; } // Навигационное свойство для медкарты
        public virtual DoctorModel doctor { get; set; }  // Навигационное свойство для врача
        public virtual DiagnosisModel diagnosis { get; set; } // Навигационное свойство для диагноза
    }
}
