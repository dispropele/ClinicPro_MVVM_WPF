using System.Windows.Input;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class AdminNavigationVM : BaseViewModel
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
    
    public ICommand DoctorsCommand { get; set; }
    public ICommand SettingsCommand { get; set; }
    public ICommand AccountCommand { get; set; }
    
    private void Doctors(object obj) => CurrentView = new Doctors();
    private void Settings(object obj) => CurrentView = new SettingsView();
    private void Account(object obj) => CurrentView = new AccountView();

    public AdminNavigationVM()
    {
        DoctorsCommand = new RelayCommand(Doctors);
        SettingsCommand = new RelayCommand(Settings);
        AccountCommand = new RelayCommand(Account);
        
        CurrentView = new Doctors();
    }
}