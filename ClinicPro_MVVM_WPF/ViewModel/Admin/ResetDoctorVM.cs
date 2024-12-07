using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class ResetDoctorVM : BaseViewModel, IDataErrorInfo
{
    private DoctorsVM _parentVm;
    private DoctorModel Doctor { get; set; }
    
    public ICommand BackToCommand { get; set; }
    public ICommand ResetPasswordCommand { get; set; }
    
    private readonly ClinicDbContext _context;
    private UserRepository _userRep;

    public ResetDoctorVM(DoctorsVM parentVm, DoctorModel doctor)
    {
        _parentVm = parentVm;
        Doctor = doctor;
        
        _context = new ClinicDbContext();
        _userRep = new UserRepository(_context);
        
        BackToCommand = new RelayCommand(BackTo);
        ResetPasswordCommand = new RelayCommand( async o => await ResetPassword(o), IsCorrect);
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
    
    private void BackTo(object obj) => _parentVm.CurrentView = new ViewDoctorData(_parentVm, Doctor.DoctorId);

    private async Task ResetPassword(object obj)
    {
        try
        {
            if (Errors != null && Errors.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var user = Doctor.User;
            await _userRep.UpdateUserPasswordAsync(user, Password);

            MessageBox.Show("Пароль успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
            BackToCommand.Execute(null);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка обновления пароля: "+e.Message);
        }
    }
    

}