using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

public class MedCardParentVM : BaseViewModel
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
    
    private int PatientId { get; set; }
    
    public void ShowParentView(object view) => CurrentView = new MedCardPatient(this, PatientId);

    public MedCardParentVM(int patientId)
    {
        PatientId = patientId;
        
        CurrentView = new MedCardPatient(this, PatientId);
    }
    
}