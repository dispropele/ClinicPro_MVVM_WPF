using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.User;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class ForgotPasswordVM : BaseViewModel
{
    private AuthVM _parentVm;
    
    public ICommand GoToResetPasswordCommand{ get; set; }
    public ICommand BackToCommand { get; set; }

    private readonly ClinicDbContext _context;
    private PatientRepository _repPatient;
    private UserRepository _repUser;
    
    private PatientModel Patient { get; set; }
    private UserModel User { get; set; }
    
    public ForgotPasswordVM(AuthVM parentVm)
    {
        _parentVm = parentVm;
        
        _context = new ClinicDbContext();
        _repPatient = new PatientRepository(_context);
        _repUser = new UserRepository(_context);
        
        BackToCommand = new RelayCommand(BackTo);
        GoToResetPasswordCommand = new RelayCommand(async o => await GoToResetPassword(o));
    }

    private void BackTo(object obj) => _parentVm.ShowLogin(obj);

    private async Task GoToResetPassword(object obj)
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(PolisNumber))
        {
            ErrorMessage = "Пустое поле";
            return;
        }
        
        try
        {
            User = await _repUser.GetUserByLoginAsync(Login);
            if (User == null)
            {
                ErrorMessage = "Пользователь с таким логином не найден";
                return;
            }

            Patient = await _repPatient.GetPatientByUserIdAsync(User.UserId);
            if (Patient == null)
            {
                ErrorMessage = "Пациент не найден для данного пользователя";
                return;
            }

            if (PolisNumber != Patient.polisNumber)
            {
                ErrorMessage = "Неверный полис";
                return;
            }

            ErrorMessage = string.Empty;
            _parentVm.CurrentView = new ResetPassword(_parentVm, User);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Произошла ошибка при проверке данных.";
            Console.WriteLine("Ошибка в GoToResetPassword: " + ex.Message);
        }
    }
    
    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    
    private string _login = string.Empty;
    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }
    
    private string _polisNumber = string.Empty;
    public string PolisNumber
    {
        get => _polisNumber;
        set
        {
            _polisNumber = System.Text.RegularExpressions.Regex.Replace(value, "[^0-9]", "");
            OnPropertyChanged();
        }
    }
}