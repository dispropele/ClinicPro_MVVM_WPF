using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.MedCard;
using ClinicPro_MVVM_WPF.Data.Record;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

public class MedCardViewAllRecordVM : BaseViewModel
{
    private MedCardParentVM _parentVm;
    private int PatientId { get; set; }
    
    private readonly ClinicDbContext _context;
    private MedCardRepository _repMedCard;
    private RecordRepository _repRecord;
    
    public ICommand BackToCommand { get; set; }
    public ICommand ViewRecordCommand { get; set; }

    public MedCardViewAllRecordVM(MedCardParentVM parentVm, int patientId)
    {
        _parentVm = parentVm;
        PatientId = patientId;

        _context = new ClinicDbContext();
        _repMedCard = new MedCardRepository(_context);
        _repRecord = new RecordRepository(_context);

        BackToCommand = new RelayCommand(BackTo);
        ViewRecordCommand = new RelayCommand(ViewRecord);
        
        OriginalMedRecords = new ObservableCollection<MedRecordModel>();
        FilteredMedRecords = new ObservableCollection<MedRecordModel>();

        Task.Run(async () => await LoadData());
    }

    private async Task LoadData()
    {
        try
        {
            var medCard = await _repMedCard.GetMedCardByPatientAsync(PatientId);
            MedCard = medCard ?? throw new Exception("Не найдена медкарта!");

            var medRecord = await _repRecord.GetRecordsAsync(MedCard.medcardId);
            if (medRecord == null) throw new Exception("Не найдены записи!");

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var record in medRecord)
                {
                    OriginalMedRecords.Add(record);
                    FilteredMedRecords.Add(record);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки данных: "+ex.Message);
        }
    }
    
    private void FilterRecords()
    {
        
        if (StartDateFilter == null && EndDateFilter == null)
        {
            FilteredMedRecords = new ObservableCollection<MedRecordModel>(OriginalMedRecords); // Если оба фильтра пусты, показываем все записи
            return;
        }
        
        var filtered = OriginalMedRecords;
            
        if (StartDateFilter != null)
            filtered = new ObservableCollection<MedRecordModel>(filtered.Where(r => r.DateTime.Date >= StartDateFilter));
            
        if (EndDateFilter != null)
            filtered = new ObservableCollection<MedRecordModel>(filtered.Where(r => r.DateTime.Date <= EndDateFilter));
            
        FilteredMedRecords = new ObservableCollection<MedRecordModel>(filtered);
    }
    
    private int _index = 0;
    public int Index
    {
        get
        {   
            _index++;
            return _index;
        }
    }
    
    private DateTime? _startDateFilter;
    public DateTime? StartDateFilter
    {
        get => _startDateFilter;
        set { _startDateFilter = value; OnPropertyChanged(); FilterRecords(); }
    }

    private DateTime? _endDateFilter;
    public DateTime? EndDateFilter
    {
        get => _endDateFilter;
        set { _endDateFilter = value; OnPropertyChanged(); FilterRecords(); }
    }
    
    private ObservableCollection<MedRecordModel> _originalMedRecords;
    public ObservableCollection<MedRecordModel> OriginalMedRecords
    {
        get => _originalMedRecords;
        set
        {
            _originalMedRecords = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<MedRecordModel> _filteredMedRecords;
    public ObservableCollection<MedRecordModel> FilteredMedRecords
    {
        get => _filteredMedRecords;
        set
        {
            _filteredMedRecords = value;
            OnPropertyChanged();
        }
    }

    private MedCardModel _medCard;
    public MedCardModel MedCard
    {
        get => _medCard;
        set
        {
            _medCard = value;
            OnPropertyChanged();
        }
    }
    
    private void BackTo(object obj) => _parentVm.ShowParentView(obj);
    
    private void ViewRecord(object parameter)
    {
        if (parameter is MedRecordModel record)
        {
            _parentVm.CurrentView = new MedCardViewRecord(_parentVm, record);
        }
        else
        {
            MessageBox.Show("Ошибка: некорректная запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}