using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

public class MedCardPatientVM : BaseViewModel
{
    private MedCardParentVM _parentVm;
    private int PatientId { get; set; }
    
    public ICommand ViewRecordMedCardCommand { get; set; }
    
    private readonly ClinicDbContext _context;
    private PatientRepository _repPatient;

    public MedCardPatientVM(MedCardParentVM parentVm, int patientId)
    {
        _parentVm = parentVm;
        PatientId = patientId;
        
        _context = new ClinicDbContext();
        _repPatient = new PatientRepository(_context);
        
        ViewRecordMedCardCommand = new RelayCommand(ViewRecordMedCard);
        
        Task.Run(async () => await LoadPatient());
    }

    private async Task LoadPatient()
    {
        IsLoading = true;
        try
        {
            var patient = await _repPatient.GetPatientByIdAsync(PatientId);

            Patient = patient ?? throw new Exception("Пациента не найден");
            IsLoading = false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка загрузки пациента: "+e);
        }
    }
    
    private void ViewRecordMedCard(object obj)
    {
        _parentVm.CurrentView = new MedCardViewAllRecord(_parentVm, PatientId);
    }
    
    private bool _isLoading = false;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    
    private PatientModel _patient;
    public PatientModel Patient
    {
        get => _patient;
        set
        {
            _patient = value;
            OnPropertyChanged();
        }
    }
    
}