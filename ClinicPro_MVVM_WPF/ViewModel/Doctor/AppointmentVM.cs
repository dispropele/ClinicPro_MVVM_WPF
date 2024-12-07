using System.Collections.ObjectModel;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class AppointmentVM : BaseViewModel
    {
        private int DoctorId { get; set; }
        
        private readonly ClinicDbContext _context = new ClinicDbContext();
        private AppointmentRepository _repository;
        
        public ObservableCollection<AppointmentModel> Appointments { get; set; }
        
        
        public AppointmentVM(int doctorId)
        {
            DoctorId = doctorId;
            
            _repository = new AppointmentRepository(_context);

            Appointments = new ObservableCollection<AppointmentModel>();
            
            LoadAppointments();
            
        }

        private async Task LoadAppointments()
        {
            try
            {
                var appointments = await _repository.GetAppointmentsByDoctorIdAsync(DoctorId);
                if (appointments != null)
                {
                    Appointments.Clear();
                    foreach (var appointment in appointments)
                    {
                        if (appointment.DateTime >= DateTime.Today)
                            Appointments.Add(appointment);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка загрузки данных: " + e.Message);
            }
            
        }
        
        
    }
}
