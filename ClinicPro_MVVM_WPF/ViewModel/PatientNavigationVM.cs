using System.Windows.Input;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient;
using ClinicPro_MVVM_WPF.View.Patient.Account;
using ClinicPro_MVVM_WPF.View.Patient.Appointment;
using ClinicPro_MVVM_WPF.View.Patient.Home;
using ClinicPro_MVVM_WPF.View.Patient.MedCard;
using ClinicPro_MVVM_WPF.ViewModel.Patient;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class PatientNavigationVM : BaseViewModel
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
    
    public ICommand HomeCommand { get; set; }
    public ICommand AccountCommand { get; set; }
    public ICommand AppointmentCommand { get; set; }
    public ICommand MedCardCommand { get; set; }
    
    private void Home(object obj) => CurrentView = new Home(PatientId);
    private void Account(object obj) => CurrentView = new AccountParent(PatientId);
    private void Appointment(object obj) => CurrentView = new AppointmentParent(PatientId);
    private void MedCard(object obj) => CurrentView = new MedCardParent(PatientId);
    
    private int PatientId { get; set; }

    public PatientNavigationVM(int patientId)
    {
        HomeCommand = new RelayCommand(Home);
        AccountCommand = new RelayCommand(Account);
        AppointmentCommand = new RelayCommand(Appointment);
        MedCardCommand = new RelayCommand(MedCard);
        
        PatientId = patientId;
        
        CurrentView = new Home(patientId);
    }

}