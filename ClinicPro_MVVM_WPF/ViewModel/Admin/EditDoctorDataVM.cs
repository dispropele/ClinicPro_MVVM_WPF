using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Data.Specialization;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class EditDoctorDataVM : BaseViewModel, IDataErrorInfo
{
    public ICommand BackToCommand { get; set; }
    public ICommand EditDoctorCommand { get; set; }
    
    public DoctorModel Doctor { get; set; }
    
    private DoctorsVM _parentVm;
    
    private readonly ClinicDbContext _context;
    private DoctorRepository _repDoctor;
    private SpecializationRepository _repSpec;
    private UserRepository _userRep;
    
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, bool> _fieldModified = new Dictionary<string, bool>();
    public string Error => null;
    
    private bool IsCorrect(object param)
    {
        return !Errors.Any() || Errors == null;
    }

    private async Task EditDoctor(object param)
    {
        try
        {
            var result = MessageBox.Show("Вы уверены, что хотите сохранить данные?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.No) return;
            
            if (Errors != null && Errors.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var user = Doctor.User;
            
            if (await _userRep.IsLoginExistsAsync(Login))
            {
                if (user.Login != Login)
                {
                    MessageBox.Show("Этот логин уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            
            user.Login = Login;
            await _userRep.UpdateUserLoginAsync(user);
            
            // Получаем существующую специализацию из контекста
            var existingSpec = await _context.Specialization
                .FirstOrDefaultAsync(x => x.NameSpecialization == FilterText);
        
            if (existingSpec == null)
            {
                var newSpec = new SpecializationModel { NameSpecialization = FilterText };
                _context.Specialization.Add(newSpec);
                await _context.SaveChangesAsync();
                Doctor.SpecializId = newSpec.SpecializId;
            }
            else
            {
                Doctor.SpecializId = existingSpec.SpecializId;
                // Отключаем отслеживание старой специализации
                _context.Entry(Doctor.Specialization).State = EntityState.Detached;
                Doctor.Specialization = existingSpec;
            }
            
            Doctor.FirstName = FirstName;
            Doctor.LastName = LastName;
            Doctor.Patronymic = Patronymic;
            Doctor.Gender = IsMale ? 'M' : 'F';
            
            await _repDoctor.UpdateDoctorAsync(Doctor);
            
            MessageBox.Show("Врач успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            BackToCommand?.Execute(null);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка обновления врача:" + e.Message);
            MessageBox.Show($"Произошла ошибка при обновлении данных: {e.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
                case nameof(FirstName):
                    if (string.IsNullOrWhiteSpace(FirstName))
                        error = "Введите имя";
                    else if (FirstName.Any(char.IsDigit))
                        error = "Без цифр";
                    break;
                case nameof(Patronymic):
                    if (Patronymic.Any(char.IsDigit))
                        error = "Без цифр";
                    break;
                case nameof(LastName):
                    if (string.IsNullOrWhiteSpace(LastName))
                        error = "Введите фамилию";
                    else if (LastName.Any(char.IsDigit))
                        error = "Без цифр";
                    break;
                case nameof(Login):
                    if (string.IsNullOrWhiteSpace(Login))
                        error = "Введите логин";
                    break;
                case nameof(FilterText):
                    if (string.IsNullOrWhiteSpace(FilterText))
                        error = "Выберите специальность";
                    else if (FilterText.Any(char.IsDigit))
                        error = "Без цифр";
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
    
    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            // Устанавливаем флаг модификации
            _fieldModified[nameof(FirstName)] = true;
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
            _fieldModified[nameof(LastName)] = true;
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
            _fieldModified[nameof(Patronymic)] = true;
            OnPropertyChanged();
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
    
    //Имя специальности
    private string _filterText;
    public string FilterText
    {
        get => _filterText;
        set
        {
            _filterText = value;
            _fieldModified[nameof(FilterText)] = true;
            OnPropertyChanged();
            LoadFilteredSpecsAsync(value);
        }
    }
    
    private string _login;
    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            _fieldModified[nameof(Login)] = true;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<string> _filteredSpecialties;
    public ObservableCollection<string> FilteredSpecialties
    {
        get => _filteredSpecialties;
        set
        {
            _filteredSpecialties = value;
            OnPropertyChanged();
        }
    }
    
    private async Task LoadFilteredSpecsAsync(string textFilter)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(textFilter))
            {
                return;
            }
            var filter = await _repSpec.GetFilteredSpecializationsAsync(textFilter);
            if (!Enumerable.Any(filter))
            {
                foreach (var special in filter)
                {
                    FilteredSpecialties.Add(special.NameSpecialization);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки данных специальностей: "+e.Message);
        }
    }
    
    private ObservableCollection<string> _originalSpecializations;
    public ObservableCollection<string> OriginalSpecializations
    {
        get => _originalSpecializations;
        set
        {
            _originalSpecializations = value;
            OnPropertyChanged();
        }
    }
    
    private async Task LoadAllSpecialtiesAsync()
    {
        try
        {
            using (var context = new ClinicDbContext())
            {
                var specializations = await new SpecializationRepository(context).GetAllSpecializationsAsync();
                if (specializations != null)
                {
                    OriginalSpecializations = new ObservableCollection<string>();
                    foreach (var special in specializations)
                    {
                        OriginalSpecializations.Add(special.NameSpecialization);
                    }
                    Console.WriteLine(OriginalSpecializations.Count);
                }
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки данных специальностей: "+e.Message);
        }
    }
    
    public EditDoctorDataVM(DoctorsVM parentVm, DoctorModel doctor)
    {
        _parentVm = parentVm;
        Doctor = doctor;

        _context = new ClinicDbContext();
        _repDoctor = new DoctorRepository(_context);
        _repSpec = new SpecializationRepository(_context);
        _userRep = new UserRepository(_context);

        LoadDoctorData();
        
        BackToCommand = new RelayCommand(BackTo);
        EditDoctorCommand = new RelayCommand(async o => await EditDoctor(o), IsCorrect);
        
        Task.Run(async () => await LoadAllSpecialtiesAsync());
        
        FilteredSpecialties = new ObservableCollection<string>();
    }

    private void BackTo(object obj) => _parentVm.CurrentView = new ViewDoctorData(_parentVm, Doctor.DoctorId);
    
    private void LoadDoctorData()
    {
        FirstName = Doctor.FirstName;
        LastName = Doctor.LastName;
        Patronymic = Doctor.Patronymic ?? string.Empty;
        IsMale = Doctor.Gender == 'M' ? true : false;
        FilterText = Doctor.Specialization.NameSpecialization;
        Login = Doctor.User.Login;
    }
    
}