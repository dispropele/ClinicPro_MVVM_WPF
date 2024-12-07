using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.User;

using Newtonsoft.Json;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class RegistrationVM : BaseViewModel, IDataErrorInfo
{
    
    public ICommand LoginCommand { get; set; }
    public ICommand RegisterCommand { get; set; }
    
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    
    private bool _isUsernameTaken;
    
    private AuthVM _parentVm;
    private UserRepository _userRep;
    private IPatientRepository _patientRep;
    
    public RegistrationVM(AuthVM parentVm, UserRepository userRep, IPatientRepository patientRep)
    {
        _parentVm = parentVm;
        _userRep = userRep;
        _patientRep = patientRep;
        
        LoginCommand = new RelayCommand(ShowLogin);
        RegisterCommand = new RelayCommand(async o => await RegisterAsync(o), CanRegister);
    }

    private void ShowLogin(object parameter)
    {
        _parentVm.ShowLogin(parameter);
    }

    private bool CanRegister(object parameter)
    {
        return !Errors.Any() || Errors == null;
    }

    private async Task RegisterAsync(object parameter)
    {
        try
        {
            if (Errors != null && Errors.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (await _userRep.IsLoginExistsAsync(Username))
            {
                MessageBox.Show("Этот логин уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Создаем нового пользователя
            await _userRep.AddUserAsync(Username, Password, "patient");

            var user = await _userRep.GetUserByLoginAsync(Username);
            if (user == null)
            {
                MessageBox.Show("Ошибка при создании пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            
            var patient = new PatientModel()
            {
                userId = user.UserId,
                firstName = FirstName,
                lastName = LastName,
                gender = IsMale ? 'M' : 'F',
                polisNumber = PolicyNumber
            };
            
            await _patientRep.AddPatientAsync(patient);

            SaveLoginDataAsync(user.UserId ,Username, "patient");

            MessageBox.Show("Регистрация успешно завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
            OpenMainWindow();

        }
        catch (Exception e)
        {
            MessageBox.Show($"Произошла ошибка при регистрации: {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }

    private void SaveLoginDataAsync(int id ,string login, string role)
    {
        var loginData = new { Id = id, Login = login, Role = role };
        string json = JsonConvert.SerializeObject(loginData);
        File.WriteAllText("login_data.json", json);
    }
    
    private void OpenMainWindow()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var mainWindow = new MainWindow();

            // Закрываем окно входа
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AuthWindow loginWindow)
                {
                    loginWindow.Close();
                    break; 
                }
            }

            mainWindow.Show();

        });
    }
    
    private string _username;
    private string _firstName;
    private string _lastName;
    private string _password;
    private string _confirmPassword;
    private string _policyNumber;
    
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));

            _ = CheckLoginAsync(value);
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }
    
    public string PolicyNumber
    {
        get => _policyNumber;
        set
        {
            _policyNumber = value;
            OnPropertyChanged(nameof(PolicyNumber));
        }
    }

    private bool _isMale = true;
    public bool IsMale
    {
        get => _isMale;
        set
        {
            _isMale = value;
            OnPropertyChanged();
        }
    }

    public bool IsFemale
    {
        get => !_isMale; 
        set
        {
            _isMale = !value;
            OnPropertyChanged(nameof(IsMale));
        }
    }
    
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }

    public string this[string columnName]
    {
        get
        {
            string error = string.Empty;
            switch (columnName)
            {
                case nameof(Username):
                    if (string.IsNullOrWhiteSpace(Username))
                        error = "Введите логин";
                    else if (_isUsernameTaken)
                        error = "Занято";
                    break;
                case nameof(FirstName):
                    if (string.IsNullOrWhiteSpace(FirstName))
                        error = "Введите имя";
                    else if (FirstName.Any(char.IsDigit))
                        error = "Без цифр)";
                    break;
                case nameof(LastName):
                    if (string.IsNullOrWhiteSpace(LastName))
                        error = "Введите фамилию";
                    else if (LastName.Any(char.IsDigit))
                        error = "Без цифр)";
                    break;
                case nameof(Password):
                    if (Password.Length < 6)
                        error = "Не менее 6 символов";
                    break;
                case nameof(ConfirmPassword):
                    if (ConfirmPassword != Password)
                        error = "Не совпадают";
                    break;
                case nameof(PolicyNumber):
                    if (string.IsNullOrWhiteSpace(PolicyNumber))
                        error = "Полис пустой";
                    else if (!PolicyNumber.All(char.IsDigit))
                        error = "Только цифры";
                    break;
 
            }

            if (!string.IsNullOrEmpty(error))
            {
                Errors[columnName] = error;
            }
            else
            {
                Errors.Remove(columnName);
            }
            
            OnPropertyChanged(nameof(Errors));
            return error;
        }
    }

    public string Error => null;

    private async Task CheckLoginAsync(string login)
    {
        _isUsernameTaken = await IsLoginExistsAsync(login);
        OnPropertyChanged(nameof(Username));
        OnPropertyChanged(nameof(Errors));
    }
    
    private async Task<bool> IsLoginExistsAsync(string login)
    {
        return await _userRep.IsLoginExistsAsync(login);
    }
    
}