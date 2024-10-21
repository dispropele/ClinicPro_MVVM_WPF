using ClinicPro_MVVM_WPF.Model.MedCardRecord;
using ClinicPro_MVVM_WPF.Model.Patient;

namespace ClinicPro_MVVM_WPF.Model.MedCard
{
    public class MedCardModel
    {
        public int MedCardId { get; set; }
        public DateTime StartDate { get; set; }
        public int PatientId { get; set; } // Внешний ключ для пациента

        public virtual PatientModel Patient { get; set; } // Навигационное свойство для пациента
        public virtual ICollection<MedRecordModel>? MedRecords { get; set; } = new HashSet<MedRecordModel>(); // Записи в медкарте

    }
}
