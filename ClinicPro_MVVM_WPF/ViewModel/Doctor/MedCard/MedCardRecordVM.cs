using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Record;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardRecordVM : BaseViewModel
{
    private MedCardVM _parentVm;

    private int RecordId { get; set; }
    private MedCardAllRecordVM _allRecordVM;
    
    public ICommand BackToCommand { get; set; }
    
    private RecordRepository _repository = new RecordRepository(new ClinicDbContext());

    private MedRecordModel _record;
    public MedRecordModel Record
    {
        get => _record;
        set
        {
            _record = value;
            OnPropertyChanged();
        }
    }
    
    public MedCardRecordVM(MedCardVM parentVm, MedCardAllRecordVM allRecordVm, int recordId)
    {
        _parentVm = parentVm;
        RecordId = recordId;
        _allRecordVM = allRecordVm;
        
        BackToCommand = new RelayCommand(o => ShowAllMedRecords(o));

        _ = LoadRecordAsync();

    }

    private void ShowAllMedRecords(object obj) => _parentVm.CurrentMedCardView = new MedCardViewAllRecord(_parentVm, _allRecordVM.MedCardId);

    private async Task LoadRecordAsync()
    {
        try
        {
            var record = await _repository.GetRecordByIdAsync(RecordId);
            if (record == null)
            {
                throw new Exception("Не найдена запись или ошибка при передаче данных");
            }
            Record = record;
            
            DateTimeString = Record.DateTimeString;
            DoctorFio = Record.Doctor.DoctorFIO;
            ComplaintsString = Record.Complaints;
            ExaminationString = Record.Examination;
            RecommendationString = Record.Recommendation;
            DiagnosisString = Record.Diagnosis.Name;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки данных: " + ex.Message);
        }
    }
    
    private string _dateTimeString = string.Empty;
    public string DateTimeString
    {
        get => _dateTimeString;
        set
        {
            _dateTimeString = value;
            OnPropertyChanged();
        }
    }

    private string _doctorFio = string.Empty;
    public string DoctorFio
    {
        get => _doctorFio;
        set
        {
            _doctorFio = value;
            OnPropertyChanged();
        }
    }
    
    private string _complaintsString = string.Empty;
    public string ComplaintsString
    {
        get => _complaintsString;
        set
        {
            _complaintsString = value;
            OnPropertyChanged();
        }
    }
    
    private string _examinationString = string.Empty;
    public string ExaminationString
    {
        get => _examinationString;
        set
        {
            _examinationString = value;
            OnPropertyChanged();
        }
    }
    
    private string _recommendationString = string.Empty;
    public string RecommendationString
    {
        get => _recommendationString;
        set
        {
            _recommendationString = value;
            OnPropertyChanged();
        }
    }
    
    private string _diagnosisString = string.Empty;
    public string DiagnosisString
    {
        get => _diagnosisString;
        set
        {
            _diagnosisString = value;
            OnPropertyChanged();
        }
    }
}