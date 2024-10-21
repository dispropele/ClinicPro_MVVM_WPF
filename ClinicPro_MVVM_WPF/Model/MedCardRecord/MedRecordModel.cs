using ClinicPro_MVVM_WPF.Model.Diagnosis;
using ClinicPro_MVVM_WPF.Model.Doctor;
using ClinicPro_MVVM_WPF.Model.MedCard;

namespace ClinicPro_MVVM_WPF.Model.MedCardRecord
{
    public class MedRecordModel
    {
        public int RecordId { get; set; }
        public int MedCardId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
        public string Complaints { get; set; }
        public string Examination { get; set; }
        public string Recommendation { get; set; }
        public string Treatment { get; set; }

        public virtual MedCardModel MedCard { get; set; } // Навигационное свойство для медкарты
        public virtual DoctorModel Doctor { get; set; }  // Навигационное свойство для врача
        public virtual DiagnosisModel Diagnosis { get; set; } // Навигационное свойство для диагноза
    }
}
