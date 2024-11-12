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
    private static MedCardVM _parentVM = new MedCardVM();
    
    public MedCardInfoVM(MedCardVM parentVM)
    {
        CurrentPatientId = 1;
        _parentVM = parentVM;
        
        Task.Run(()=>LoadMedCardAsync(CurrentPatientId));
    }
    

    public ICommand ShowMenuMedCardCommand => new RelayCommand(o => _parentVM.ShowMenuMedCard(o));
    
    private void UpdateMedCardFio()
    {
        if (CurrentMedCard?.Patient == null)
        {
            MedCardFio = "Неизвестный пациент";
            Console.WriteLine("NOOO PATIENT");
        }
        else
        {
            MedCardFio =
                $"{CurrentMedCard.Patient.LastName} {CurrentMedCard.Patient.FirstName} {CurrentMedCard.Patient.Patronymic}";
            Console.WriteLine("UpdateMedCardFio");
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

    private async Task LoadMedCardAsync(int patientId)
    {
        IsLoading = true;
        try
        {
            using (var context = new ClinicDbContext())
            {
                var repository = new MedCardRepository(context);
                MedCardModel? medCard = await repository.GetMedCardByPatientAsync(patientId);
                CurrentMedCard = medCard;
                
                if (CurrentMedCard != null) // Добавлена проверка на null
                {
                    UpdateMedCardFio();
                    Console.WriteLine("Zaversheno.");
                }
                else
                { 
                    Console.WriteLine("MedCard не найден.");
                    MedCardFio = "Нет ФИО"; // Или другое сообщение
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
            OnPropertyChanged(nameof(IsLoading));
        }
    }
    
}