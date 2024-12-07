using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Data.Specialization;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class AddDoctorVM : BaseViewModel, IDataErrorInfo
{
    private DoctorsVM _parentVm;
    
    public ICommand BackToCommand { get; set; }
    public ICommand AddDoctorCommand { get; set; }

    private readonly ClinicDbContext _context;
    private SpecializationRepository _repSpec;
    private DoctorRepository _repDoctor;
    private UserRepository _userRep;
    
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
    
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, bool> _fieldModified = new Dictionary<string, bool>();
    
    private async Task LoadAllSpecialtiesAsync()
    {
        try
        {
            var specializations = await _repSpec.GetAllSpecializationsAsync();
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
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки данных специальностей: "+e.Message);
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
    
    private async Task AddDoctor(object obj)
    {
        try
        {
            if (Errors != null && Errors.Any())
            {
                MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (await _userRep.IsLoginExistsAsync(Login))
            {
                MessageBox.Show("Этот логин уже занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            //Создаем нового врача
            await _userRep.AddUserAsync(Login, Password, "doctor");

            var user = await _userRep.GetUserByLoginAsync(Login);
            if (user == null)
            {
                MessageBox.Show("Ошибка при создании пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            await GetSpecializationIdAsync(FilterText);
            var doctor = new DoctorModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                Patronymic = Patronymic,
                Gender = IsMale ? 'M' : 'F',
                userId = user.UserId,
                SpecializId = _specializationId
            };

            await _repDoctor.AddDoctorAsync(doctor);
            
            MessageBox.Show("Доктор успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearFields();
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка добавления врача:"+e.Message);
        }
    }

    private void ClearFields()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Patronymic = string.Empty;
        IsMale = true;
        FilterText = string.Empty;
        Login = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }
    
    private int _specializationId;
    private async Task GetSpecializationIdAsync(string textFilter)
    {
        var spec = _context.Specialization.FirstOrDefault(x => x.NameSpecialization == textFilter);
        
        if (spec == null)
        {
            var newSpec = new SpecializationModel() { NameSpecialization = textFilter };
            _context.Specialization.Add(newSpec);
            await _context.SaveChangesAsync();
            _specializationId = newSpec.SpecializId;
            return;
        }

        _specializationId = spec.SpecializId;
    }
    
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
    
    
    public AddDoctorVM(DoctorsVM parentVm)
    {
        _parentVm = parentVm;

        BackToCommand = new RelayCommand(_parentVm.ShowListDoctor);
        AddDoctorCommand = new RelayCommand(async o => await AddDoctor(o), IsCorrect);
        
        _context = new ClinicDbContext();
        _repSpec = new SpecializationRepository(_context);
        _userRep = new UserRepository(_context);
        _repDoctor = new DoctorRepository(_context);
        
        Task.Run(async () => LoadAllSpecialtiesAsync());
        
        FilteredSpecialties = new ObservableCollection<string>();
    }
    
}