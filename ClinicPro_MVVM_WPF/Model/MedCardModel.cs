
namespace ClinicPro_MVVM_WPF.Model
{
    public class MedCardModel
    {
        public int medcardId { get; set; }
        
        public DateTime startDate { get; set; }
        
        public int patientId { get; set; } // Внешний ключ для пациента

        public virtual PatientModel patient { get; set; } // Навигационное свойство для пациента

    }
}
