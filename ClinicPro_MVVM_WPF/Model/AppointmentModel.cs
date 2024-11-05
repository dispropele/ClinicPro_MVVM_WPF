
namespace ClinicPro_MVVM_WPF.Model
{
    public class AppointmentModel
    {

        public int appointmentId { get; set; }
        
        public int patientId { get; set; }
        
        public int doctorId { get; set; }
        
        public DateTime dateTime { get; set; }
        
        public string status { get; set; } // "Запланирован", "Выполнен", "Отменен"

        public string? reasonCancel { get; set; }


        public virtual PatientModel patient { get; set; } = null!; // Навигационное свойство для пациента
        public virtual DoctorModel doctor { get; set; } = null!; // Навигационное свойство для врача

    }
}
