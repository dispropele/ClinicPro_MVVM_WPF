using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Account;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Account;

public partial class AccountViewVM : BaseViewModel, IDataErrorInfo
{
    private int PatientId { get; set; }
    private AccountParentVM _parentVm;
    
    public ICommand ExitToLoginCommand { get; set; }
    public ICommand ChangeDataCommand { get; set; }
    public ICommand ResetPasswordCommand { get; set; }
    public ICommand SaveChangesCommand { get; set; }

    private PatientRepository _repPatient;
    
    private PatientModel Patient { get; set; }
    
    public AccountViewVM(AccountParentVM parentVm, int patientId)
    {
        PatientId = patientId;
        _parentVm = parentVm;

        var context = new ClinicDbContext();
        _repPatient = new PatientRepository(context);
        
        ExitToLoginCommand = new RelayCommand(ExitToLogin);
        ChangeDataCommand = new RelayCommand(ChangeData);
        SaveChangesCommand = new RelayCommand(async o => await SaveChanges(o), IsCorrect);
        ResetPasswordCommand = new RelayCommand(ResetPassword);
        
        Task.Run(async () => await LoadAccountData());
    }

    private void ResetPassword(object o) => _parentVm.CurrentView = new AccountResetPassword(_parentVm, Patient);

    private async Task SaveChanges(object obj)
    {
        try
        {
            if (Errors != null && Errors.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                
            var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            Patient.dateOfBirth = DateOfBirth;
            Patient.email = Email;
            Patient.phoneNumber = PhoneNumber;
                
            await _repPatient.UpdatePatientAsync(Patient);
            MessageBox.Show("Успешное обновление данных");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка сохранения изменений: "+ ex.Message);
        }
    }

    private async Task LoadAccountData()
    {
        try
        {
            await LoadPatient();
            
            DateOfBirth = Patient.dateOfBirth!.Value;
            PhoneNumber = Patient.phoneNumber ?? string.Empty;
            Email = Patient.email ?? string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки данных аккаунта: "+e.Message);
        }
    }
    
    private async Task LoadPatient()
    {
        try
        {
            var patient = await _repPatient.GetPatientByIdAsync(PatientId);
            Patient = patient ?? throw new Exception("Был передан пустой пациент");
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки пациента: "+e.Message);
        }
    }

    private void ChangeData(object obj)
    {
        IsReadOnly = !IsReadOnly;
        IsEnable = !IsEnable;
    }
    
    private void ExitToLogin(object obj)
    {
        var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            string filePath = "login_data.json";
            try
            {
                // Удаление файла login_data.json
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                Thread.Sleep(1000);
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                // Обработка ошибки удаления файла (логирование, сообщение пользователю)
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }
    }
    
    private bool _isEnable;
    public bool IsEnable
    {
        get => _isEnable;
        set
        {
            _isEnable = value;
            OnPropertyChanged();
        }
    }

    private bool _isReadOnly = true;
    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            _isReadOnly = value;
            ChangeMessage = !_isReadOnly ? "Режим редактирования!" : string.Empty;
            OnPropertyChanged();
        }
    }

    private string _changeMessage;
    public string ChangeMessage
    {
        get => _changeMessage;
        set
        {
            _changeMessage = value;
            OnPropertyChanged();
        }
    }
    
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            _fieldModified[nameof(DateOfBirth)] = true;
            OnPropertyChanged();
        }
    }

    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            _fieldModified[nameof(PhoneNumber)] = true;
            OnPropertyChanged();
        }
    }
    
    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            _fieldModified[nameof(Email)] = true;
            OnPropertyChanged();
        }
    }
    
    private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string PhoneRegex = @"^\+?[0-9]{10,15}$";
        
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        private Dictionary<string, bool> _fieldModified = new Dictionary<string, bool>();
        
        [GeneratedRegex(PhoneRegex)]
        private static partial Regex MyPhoneRegex();
        [GeneratedRegex(EmailRegex)]
        private static partial Regex MyEmailRegex();
        
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
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "Введите почту";
                        else if (!MyEmailRegex().IsMatch(Email))
                            error = "Некорректная почта";
                        break;
                    case nameof(PhoneNumber):
                        if (string.IsNullOrWhiteSpace(PhoneNumber))
                            error = "Введите номер";
                        else if (!MyPhoneRegex().IsMatch(PhoneNumber))
                            error = "Некорректный номер";
                        break;
                    case nameof(DateOfBirth):
                        if (string.IsNullOrWhiteSpace(DateOfBirth.ToString()))
                            error = "Введите дату";
                        else if (DateOfBirth > DateTime.Now)
                            error = "Введите корректную дату";
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
        
        private bool IsCorrect(object param)
        {
            return !Errors.Any() || Errors == null;
        }
    
}