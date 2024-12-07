using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class DoctorsVM : BaseViewModel
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
    
    public ICommand DeleteDoctorDataCommand { get; set; }

    public void ShowListDoctor(object obj) => CurrentView = new DoctorsView(this);
        
    public DoctorsVM()
    {
        CurrentView = new DoctorsView(this);
        
        DeleteDoctorDataCommand = new RelayCommand(DeleteDoctor);

    }

    private void DeleteDoctor(object obj)
    {
        MessageBox.Show("Уверены что хотите удалить данного врача?");
    }
    
}