using System.IO;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.User;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class LoginVM : BaseViewModel
{
    private string _login;
    private string _password;
    private bool _isLoginSuccessful = false; // Флаг успешного входа
    private string _errorMessage = "";       // Сообщение об ошибке

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ForgotPasswordCommand { get; }

    private readonly UserRepository _rep = new UserRepository(new ClinicDbContext());

    private AuthVM _parentVm;
    
    public LoginVM(AuthVM parentVm)
    {
        _parentVm = parentVm;
        
        LoginCommand = new RelayCommand(async _ => await LoginAsync(Login, Password));
        RegisterCommand = new RelayCommand(ShowRegister);
        ForgotPasswordCommand = new RelayCommand(ForgotPassword);
        
        
        
        // Проверяем файл на наличие данных о предыдущем входе
        LoadLoginData();
        if (_isLoginSuccessful)
        {
            // Если вход выполнен ранее, сразу переходим на главное окно
            OpenMainWindow();
        }

    }

    private void ForgotPassword(object obj)
    {
        _parentVm.CurrentView = new ForgotPassword(_parentVm);
    }

    private void ShowRegister(object obj)
    {
        _parentVm.ShowRegistration(obj);
    }
    

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
            ErrorMessage = ""; // Очищаем сообщение об ошибке при изменении логина
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ErrorMessage = ""; // Очищаем сообщение об ошибке при изменении пароля
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    private async Task LoginAsync(string login, string password)
    {
        try
        {
            //получение логина
            var user = await _rep.GetUserByLoginAsync(login);
            
            if (user == null)
            {
                ErrorMessage = "Такого пользователя не существует";
                return;
            }
            
            //проверка пароля
            var valid = _rep.VerifyPassword(password, user.HashPassword);
            
            if (Login == user.Login && valid)
            {
                _isLoginSuccessful = true;
                
                // Сохраняем данные для автоматического входа
                SaveLoginData(user.UserId ,user.Login, user.Role); // "doctor" - пример роли

                // Открываем главное окно
                OpenMainWindow();
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка входа";
        }
    }

    private void SaveLoginData(int id, string login, string role)
    {
        var loginData = new LoginData {Id = id, Login = login, Role = role };
        string json = JsonConvert.SerializeObject(loginData);
        File.WriteAllText("login_data.json", json);
    }

    private void LoadLoginData()
    {
        if (File.Exists("login_data.json"))
        {
            string json = File.ReadAllText("login_data.json");
            try
            {
                var loginData = JsonConvert.DeserializeObject<LoginData>(json);
                _login = loginData.Login;
                _isLoginSuccessful = true;

            }
            catch (JsonException)
            {
                // Обработка ошибки десериализации (например, поврежденный файл)
                _isLoginSuccessful = false;
            }
        }
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

    // Класс для хранения данных входа
    private class LoginData
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }
}