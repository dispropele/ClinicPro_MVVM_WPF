using ClinicPro_MVVM_WPF.Model.Patient;
using ClinicPro_MVVM_WPF.Model.Doctor;

namespace ClinicPro_MVVM_WPF.Model.Appointment
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus Status { get; set; } // "Запланирован", "Выполнен", "Отменен"
        public string? ReasonCancel { get; set; }


        public virtual PatientModel Patient { get; set; } = new PatientModel(); // Навигационное свойство для пациента
        public virtual DoctorModel Doctor { get; set; } // Навигационное свойство для врача


        public AppointmentModel() { }

        public AppointmentModel(int patientId, int doctorId, DateTime date, AppointmentStatus status)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            DateTime = date;
            Status = status;
        }

        public enum AppointmentStatus
        {
            Scheduled,   // Запланирован
            Completed,   // Выполнен
            Canceled     // Отменен
        }
    }
}
