using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class ViewDoctorDataVM : BaseViewModel
{
    private DoctorsVM _parentVm { get; set; }
    
    public ICommand BackToCommand { get; set; }
    public ICommand ResetPasswordCommand { get; set; }
    public ICommand DeleteThisDoctorCommand { get; set; }
    public ICommand EditDataCommand { get; set; }
    
    private int DoctorId { get; set; }
    private DoctorModel Doctor { get; set; }
    
    private readonly ClinicDbContext _context = new ClinicDbContext();
    private readonly DoctorRepository _repDoctor;

    public ViewDoctorDataVM(DoctorsVM parentVm, int doctorId)
    {
        _parentVm = parentVm;
        DoctorId = doctorId;
        _repDoctor = new DoctorRepository(_context);
        
        BackToCommand = new RelayCommand(o => _parentVm.ShowListDoctor(o));
        DeleteThisDoctorCommand = new RelayCommand(async o => await DeleteThisDoctor(o));
        ResetPasswordCommand = new RelayCommand(ResetPassword);
        
        LoadDoctorDataAsync();
        
        EditDataCommand = new RelayCommand(o => _parentVm.CurrentView = new EditDoctorData(_parentVm, Doctor));
    }

    private void ResetPassword(object obj) => _parentVm.CurrentView = new ResetPasswordDoctor(_parentVm, Doctor);
    
    
    private async Task DeleteThisDoctor(object obj)
    {
        //MessageBox о подтверждении
        var result = MessageBox.Show("Вы уверены, что хотите удалить данного врача? При удалении врача, будут удалены все записи к нему!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            await _repDoctor.DeleteDoctorAsync(DoctorId);
            MessageBox.Show("Врач успешно удален!");
            
        }
        
    }

    private async Task LoadDoctorDataAsync()
    {
        try
        {
            var doctor = await _repDoctor.GetDoctorByIdAsync(DoctorId);
            if (doctor != null)
            {
                UpdateDataDoctor(doctor);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки данных: "+ex.Message);
        }
    }

    private void UpdateDataDoctor(DoctorModel doctor)
    {
        Doctor = doctor;
        FirstName = doctor.FirstName;
        LastName = doctor.LastName;
        Patronymic = doctor.Patronymic ?? "Отсутствует";
        Gender = doctor.Gender.ToString();
        Specialization = doctor.Specialization.NameSpecialization;
        Login = doctor.User.Login;
    }
    
    
    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }
    
    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
        }
    }

    private string _patronymic;
    public string Patronymic
    {
        get => _patronymic;
        set
        {
            _patronymic = value;
            OnPropertyChanged();
        }
    }

    private string _gender;
    public string Gender
    {
        get => _gender;
        set
        {
            _gender = value;
            OnPropertyChanged();
        }
    }

    private string _specialization;
    public string Specialization
    {
        get => _specialization;
        set
        {
            _specialization = value;
            OnPropertyChanged();
        }
    }

    private string _login;
    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }

}