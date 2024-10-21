using ClinicPro_MVVM_WPF.Model.Patient;
using ClinicPro_MVVM_WPF.Model.Doctor;

namespace ClinicPro_MVVM_WPF.Model.Appointment
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; } // Используем TimeSpan для времени
        public AppointmentStatus Status { get; set; } // "Запланирован", "Выполнен", "Отменен"
        public string? ReasonCancel { get; set; }


        public virtual PatientModel Patient { get; set; } = new PatientModel(); // Навигационное свойство для пациента
        public virtual DoctorModel Doctor { get; set; } // Навигационное свойство для врача


        public AppointmentModel() { }

        public AppointmentModel(int patientId, int doctorId, DateTime date, TimeSpan time, AppointmentStatus status)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            Date = date;
            Time = time;
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
