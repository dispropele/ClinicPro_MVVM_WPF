using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.Appointment;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;

        public AppointmentRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync()
        {
            return await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<AppointmentModel?> GetAppointmentByIdAsync(int? appointmentId)
        {
            return await _context.Appointment
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _context.Appointment
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsForDoctorTodayAsync(int doctorId)
        {
            var today = DateTime.Today;
            return await _context.Appointment
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId && a.DateTime.Date == today)
                .ToListAsync();
        }
        
        public async Task AddAppointmentAsync(AppointmentModel appointment)
        {
            await _context.Appointment.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(AppointmentModel appointment)
        {
            _context.Appointment.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointment.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> IsPatientInProgressAsync(int doctorId, int patientId)
        {
            return await _context.Appointment
                .AnyAsync(a => a.DoctorId == doctorId && a.PatientId == patientId && a.Status == "InProgress");
        }
        
}