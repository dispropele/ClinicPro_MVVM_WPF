using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.User;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class ResetPasswordVM : BaseViewModel, IDataErrorInfo
{
    private AuthVM _parentVm;
    
    private readonly ClinicDbContext _context;
    private UserRepository _repUser;
    
    public ICommand BackToCommand { get; set; }
    public ICommand ResetPasswordCommand { get; set; }
    
    private UserModel _user;
    
    public ResetPasswordVM(AuthVM parentVm, UserModel user)
    {
        _parentVm = parentVm;
        _user = user;
        
        _context = new ClinicDbContext();
        _repUser = new UserRepository(_context);
        
        BackToCommand = new RelayCommand(BackTo);
        ResetPasswordCommand = new RelayCommand(async o => await ResetPassword(o), IsCorrect);
    }
    
    private void BackTo(object obj) => _parentVm.CurrentView = new ForgotPassword(_parentVm);

    private async Task ResetPassword(object obj)
    {
        try
        {
            await _repUser.UpdateUserPasswordAsync(_user, Password);
            MessageBox.Show("Пароль успешно изменен!");
            _parentVm.ShowLogin(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка обновления пароля: "+e.Message);
        }
    }
    
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, bool> _fieldModified = new Dictionary<string, bool>();
    
    private bool IsCorrect(object param)
    {
        return !Errors.Any() || Errors == null;
    }
    
    public string this[string columnName]
    {
        get
        {
            string error = string.Empty;
            
            // Добавляем проверку на модификацию поля
            if (!_fieldModified.ContainsKey(columnName) || !_fieldModified[columnName])
                return string.Empty;
            
            switch (columnName)
            {
                case nameof(Password):
                    if (Password.Length < 6)
                        error = "Не менее 6 символов";
                    break;
                case nameof(ConfirmPassword):
                    if (ConfirmPassword != Password)
                        error = "Не совпадают";
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
    
    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            _fieldModified[nameof(Password)] = true;
            OnPropertyChanged();
        }
    }
    
    private string _confirmPassword;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            _fieldModified[nameof(ConfirmPassword)] = true;
            OnPropertyChanged();
        }
    }

}