using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

public class AppointmentCancelVM : BaseViewModel
{
    private AppointmentParentVM _parentVM;
    private AppointmentModel Appointment { get; set; }
    
    public ICommand CancelAppointmentCommand { get; set; }
    public ICommand BackToCommand { get; set; }
    
    private ClinicDbContext _context;
    private AppointmentRepository _repAppointment;
    
    public AppointmentCancelVM(AppointmentParentVM parentVm, AppointmentModel model)
    {
        _parentVM = parentVm;
        Appointment = model;

        BackToCommand = new RelayCommand(BackTo);
        CancelAppointmentCommand = new RelayCommand(async o => await CancelAppointment(o));

        _context = new ClinicDbContext();
        _repAppointment = new AppointmentRepository(_context);
    }

    private string _reasonCancelText = string.Empty;
    public string ReasonCancelText
    {
        get => _reasonCancelText;
        set
        {
            _reasonCancelText = value;
            OnPropertyChanged();
        }
    }

    private async Task CancelAppointment(object obj)
    {
        try
        {
            Appointment.Status = "Canceled";
            Appointment.ReasonCancel = string.IsNullOrWhiteSpace(ReasonCancelText) ? "Отсутствует" : ReasonCancelText;
            
            await _repAppointment.UpdateAppointmentAsync(Appointment);
            
            MessageBox.Show("Запись успешно отменена!");
            BackToCommand.Execute(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка отмены записи: "+e.Message);
        }
        
        
    }
    
    private void BackTo(object obj) => _parentVM.ShowParentView(obj);

}