using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Record;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardAllRecordVM : BaseViewModel
{
        private static ClinicDbContext _context = new ClinicDbContext();
        private RecordRepository _recordRepository = new RecordRepository(_context);

        private MedCardVM _parentVm;
        
        // ��������� �������
        private ObservableCollection<MedRecordModel> _medRecords;
        public ObservableCollection<MedRecordModel> MedRecords
        {
            get => _medRecords;
            set { _medRecords = value; OnPropertyChanged(); }
        }

        // �������� ��� ����������
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

        // �������
        public ICommand GoBackCommand { get; set; }
        public ICommand ViewRecordDetailsCommand { get; set; }

        // ������������ ��������� ������� ��� ����������
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

        private int _index = 0;
        public int Index
        {
            get
            {   
                _index++;
                return _index;
            }
        }

        public MedCardAllRecordVM(MedCardVM parentVm, int medCardId)
        {
            MedCardId = medCardId;
            _parentVm = parentVm;
            
            // ������������� ��������� � ������
            MedRecords = new ObservableCollection<MedRecordModel>();
            _originalMedRecords = new ObservableCollection<MedRecordModel>();

            GoBackCommand = new RelayCommand(GoBack); // RelayCommand - ���� ���������� ICommand
            ViewRecordDetailsCommand = new RelayCommand(ViewRecordDetails);

            // �������� ������
            LoadDataAsync();
        }

        // ����� ��� �������� ������ (�������� �� ���� ������ ��������� ������)
        private async Task LoadDataAsync()
        {
            try
            {
                Console.WriteLine("������ ��������");
                var medRecords = await _recordRepository.GetRecordsAsync(MedCardId);
                Console.WriteLine($"���������� {medRecords.Count()}");

                if (medRecords.Any() && medRecords != null)
                {
                    foreach (var record in medRecords)
                    {
                        MedRecords.Add(record);
                        OriginalMedRecords.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine("������ �� �������");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("������ �������� ������: " + ex.Message);
            }
            
        }

        // ����� ��� ���������� �������
        private void FilterRecords()
        {
            Console.WriteLine("������");
            if (StartDateFilter == null && EndDateFilter == null)
            {
                MedRecords = new ObservableCollection<MedRecordModel>(_originalMedRecords); // ���� ��� ������� �����, ���������� ��� ������
                return;
            }
            Console.WriteLine("����� �������");
            var filtered = _originalMedRecords;
            
            if (StartDateFilter != null)
                filtered = new ObservableCollection<MedRecordModel>(filtered.Where(r => r.DateTime.Date >= StartDateFilter));
            
            if (EndDateFilter != null)
                 filtered = new ObservableCollection<MedRecordModel>(filtered.Where(r => r.DateTime.Date <= EndDateFilter));
            
            MedRecords = new ObservableCollection<MedRecordModel>(filtered);
        }
        
        private int _medCardId;
        public int MedCardId
        {
            get => _medCardId;
            set
            {
                _medCardId = value;
                OnPropertyChanged();
            }
        }
        

        // ������ ��� ������
        private void GoBack(object parameter)
        {
            _parentVm.ShowInfoMedCard(parameter);
        }

        private void ViewRecordDetails(object parameter)
        {
            if (parameter is int recordId)
            {
                _parentVm.CurrentMedCardView = new MedCardViewRecord(this, _parentVm, recordId);
            }
            else
            {
                MessageBox.Show("������: ������������ ID ������", "������", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    
    
}