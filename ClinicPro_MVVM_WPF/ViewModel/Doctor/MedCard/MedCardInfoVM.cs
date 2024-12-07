using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.MedCard;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardInfoVM : BaseViewModel
{
    private static MedCardVM _parentVM;
    
    public MedCardInfoVM(MedCardVM parentVM, int patientId)
    {
        CurrentPatientId = patientId;
        _parentVM = parentVM;
        
       _ = LoadMedCardAsync(CurrentPatientId);
       
    }
    
    private MedCardViewAllRecord MedCardViewAllRecord;

    public ICommand ShowMenuMedCardCommand => new RelayCommand(o => _parentVM.ShowMenuMedCard(o));
    public ICommand ViewRecordMedCardCommand => new RelayCommand(o => ShowRecord(o));
    public ICommand CreateRecordCommand => new RelayCommand(o => CreateRecord(o));

    private void ShowRecord(object obj) => _parentVM.CurrentMedCardView = new MedCardViewAllRecord(_parentVM, CurrentMedCard.medcardId);
    private void CreateRecord(object obj) => _parentVM.CurrentMedCardView = new MedCardCreateRecord(_parentVM, _parentVM.DoctorId);
    
    private void UpdateMedCardInfo()
    {
        if (CurrentMedCard?.Patient == null)
        {
            MedCardFio = "Неизвестный пациент";
        }
        else
        {
            MedCardFio =
                $"{CurrentMedCard.Patient.lastName} {CurrentMedCard.Patient.firstName} {CurrentMedCard.Patient.patronymic}";
            Gender = CurrentMedCard.Patient.gender.ToString();
            DateOfBirth = $"{CurrentMedCard.Patient.dateOfBirth:dd.MM.yyyy}";
            PhoneNumber = CurrentMedCard.Patient.phoneNumber ?? string.Empty;
            EmailAddress = CurrentMedCard.Patient.email ?? string.Empty;
            PolisNumber = CurrentMedCard.Patient.polisNumber;
        }
    }

    private string _medCardNumber;
    public string MedCardNumber
    {
        get => _medCardNumber;
        set
        {
            _medCardNumber = value;
            OnPropertyChanged();
        }
    }
    
    private string _medCardFio;
    public string MedCardFio
    {
        get => _medCardFio;
        private set
        {
            _medCardFio = value;
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
    
    private string _dateOfBirth;
    public string DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            OnPropertyChanged();
        }
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged();
        }
    }
    
    private string _emailAddress;
    public string EmailAddress
    {
        get => _emailAddress;
        set
        {
            _emailAddress = value;
            OnPropertyChanged();
        }
    }

    private string _polisNumber;
    public string PolisNumber
    {
        get => _polisNumber;
        set
        {
            _polisNumber = value;
            OnPropertyChanged();
        }
    }
    
    private async Task LoadMedCardAsync(int patientId)
    {
        IsLoading = true;
        try
        {
            using (var context = new ClinicDbContext())
            {
                var repository = new MedCardRepository(context);
                CurrentMedCard = await repository.GetMedCardByPatientAsync(patientId);
                
                if (CurrentMedCard != null) // Добавлена проверка на null
                {
                    UpdateMedCardInfo();
                }
                else
                { 
                    MessageBox.Show("Была передана пустая мед карта", "Внимание"); // Или другое сообщение
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private MedCardModel _currentMedCard;
    public MedCardModel CurrentMedCard
    {
        get => _currentMedCard;
        set
        {
            _currentMedCard = value;
            OnPropertyChanged();
        }
    }
    
    private int _currentPatientId; // ID текущего пациента
    public int CurrentPatientId
    {
        get => _currentPatientId;
        set
        {
            _currentPatientId = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    
}