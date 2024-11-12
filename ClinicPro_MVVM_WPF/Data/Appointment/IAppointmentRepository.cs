using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Data.Appointment;

public interface IAppointmentRepository
{
    Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync();
    Task<AppointmentModel?> GetAppointmentByIdAsync(int? appointmentId);
    Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentModel>> GetAppointmentsByPatientIdAsync(int patientId);
    Task AddAppointmentAsync(AppointmentModel appointment);
    Task UpdateAppointmentAsync(AppointmentModel appointment);
    Task DeleteAppointmentAsync(int appointmentId);
    
    Task<IEnumerable<AppointmentModel>> GetAppointmentsForDoctorTodayAsync(int doctorId);
    Task<bool> IsPatientInProgressAsync(int doctorId, int patientId);
}