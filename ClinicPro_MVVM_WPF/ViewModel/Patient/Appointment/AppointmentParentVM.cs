using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Appointment;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

public class AppointmentParentVM : BaseViewModel
{
    private int PatientId { get; set; }

    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
    
    public void ShowParentView(object obj) => CurrentView = new AppointmentView(this, PatientId);
    
    public AppointmentParentVM(int patientId)
    {
        PatientId = patientId;
        
        CurrentView = new AppointmentView(this, patientId);
    }
}