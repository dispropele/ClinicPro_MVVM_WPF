using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Appointment;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

public class AppointmentViewVM : BaseViewModel
{
    private AppointmentParentVM _parentVm;
    private int PatientId { get; set; }
    
    public ICommand CancelAppointmentCommand { get; set; }
    
    private readonly ClinicDbContext _context;
    private AppointmentRepository _repAppointment;
    
    public ObservableCollection<AppointmentModel> Appointments { get; set; }
    
    public AppointmentViewVM(AppointmentParentVM parentVm, int patientId)
    {
        _parentVm = parentVm;
        PatientId = patientId;
        
        CancelAppointmentCommand = new RelayCommand(CancelAppointment);

        _context = new ClinicDbContext();
        _repAppointment = new AppointmentRepository(_context);
        
        Appointments = new ObservableCollection<AppointmentModel>();
        
        Task.Run(async () => await LoadAppointments());
    }
    
    private async Task LoadAppointments()
    {
        try
        {
            var appointments = await _repAppointment.GetAppointmentsByPatientIdInScheduledAsync(PatientId);
            
            if(appointments == null) throw new Exception("Записей не найдено!");

            await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Appointments.Clear();
                    foreach (var appointment in appointments) Appointments.Add(appointment);
                }
            );
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки данных: " + e.Message);
        }
            
    }

    private void CancelAppointment(object obj)
    {
        if (obj is AppointmentModel AppointmentModel)
        {
            _parentVm.CurrentView = new AppointmentCancel(_parentVm, AppointmentModel);
        }
        else
        {
            MessageBox.Show("Ошибка перехода к записи!");
        }
    }
    
}