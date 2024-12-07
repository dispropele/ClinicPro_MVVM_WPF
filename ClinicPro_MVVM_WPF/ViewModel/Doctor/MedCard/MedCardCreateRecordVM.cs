using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Record;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardCreateRecordVM : BaseViewModel
{
    private MedCardVM _parentVm;
    private int _doctorId;
    
    public ICommand BackToCommand { get; set; }
    public ICommand SaveRecordCommand { get; set; }
    
    private static ClinicDbContext _context = new ClinicDbContext();
    private RecordRepository _recordRep =new RecordRepository(_context);

    public MedCardCreateRecordVM(MedCardVM parentVm, int doctorId)
    {
       _parentVm = parentVm;
       _doctorId = doctorId;
       
       BackToCommand = new RelayCommand(o => _parentVm.ShowInfoMedCard(o));
       SaveRecordCommand = new RelayCommand(async o => await SaveRecord(o));
    }

    private async Task SaveRecord(object o)
    {
        //MessageBox о подтверждении
        var result = MessageBox.Show("Вы уверены, что хотите сохранить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            await GetDiagnosisId();
            var record = new MedRecordModel
            {
                MedCardId = _parentVm.MedCardInfoVM.CurrentMedCard.medcardId,
                DoctorId = _doctorId,
                DateTime = DateTime.Now,
                Complaints = ComplaintsText,
                Examination = ExaminationString,
                Recommendation = RecommendationString,
                DiagnosisId = _diagnosisId
            };
            await _recordRep.AddRecordAsync(record);
            MessageBox.Show("Запись успешно добавлена!");
            _parentVm.ShowInfoMedCard(o);
        }
        
    }

    private int _diagnosisId;
    private async Task GetDiagnosisId()
    {
        var diagnosis = _context.Diagnosis.FirstOrDefault(d => d.Name == DiagnosisString);
        if (diagnosis == null)
        {
            var newDiagnosis = new DiagnosisModel { Name = DiagnosisString };
            _context.Diagnosis.Add(newDiagnosis);
            await _context.SaveChangesAsync();
            _diagnosisId = newDiagnosis.DiagnosisId;
            return;
        }
        _diagnosisId = diagnosis.DiagnosisId;
    }

    private string _complaintsText = string.Empty;
    public string ComplaintsText
    {
        get => _complaintsText;
        set
        {
            _complaintsText = value;
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