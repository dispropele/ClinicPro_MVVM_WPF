
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Account;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Account;

public class AccountParentVM : BaseViewModel
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
    
    public void ShowParentView(object view) => CurrentView = new AccountView(this, PatientId);
    
    private int PatientId { get; set; }

    public AccountParentVM(int patientId)
    {
        CurrentView = new AccountView(this, patientId);
        
        PatientId = patientId;
    }

}