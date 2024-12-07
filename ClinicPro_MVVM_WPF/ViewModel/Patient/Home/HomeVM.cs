using System.Windows.Input;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient;
using ClinicPro_MVVM_WPF.View.Patient.Home;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Home;

public class HomeVM : BaseViewModel
{
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

    public void Home(object obj) => CurrentView = new HomeView(this, PatientId);
    
    private int PatientId { get; set; }
    
    public HomeVM(int patientId)
    {
        PatientId = patientId;
        
        CurrentView = new HomeView(this, patientId);
    }
    
    
}