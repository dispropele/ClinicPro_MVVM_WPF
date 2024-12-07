using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Home;

public class HomeViewVM : BaseViewModel
{
    private HomeVM _parentVm;
    private int PatientId { get; set; }
    
    public ICommand CreateAppointmentPatientCommand { get; set; }

    public HomeViewVM(HomeVM parentVm,int patientId)
    {
        _parentVm = parentVm;
        PatientId = patientId;

        CreateAppointmentPatientCommand = new RelayCommand(CreateAppointmentPatient);
        
        InitializeTimers(); // ����� ������ ������������� ��������
        Task.Run(async () => await LoadTodayAppointmentsAsync()); // ������������� ������ ��� �������� ViewModel
    }
    
    private void CreateAppointmentPatient(object obj) => _parentVm.CurrentView = new ChoiceDoctors(_parentVm, PatientId);
    
    private ObservableCollection<AppointmentModel> _todayAppointments;

        private string _appointmentTodayOne;
        private string _appointmentTodayTwo;
        private DispatcherTimer _timerAppointment;

        private void InitializeTimers()
        {
            // ������������� ������� ��� ���������� ������
            _timerAppointment = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            _timerAppointment.Tick += OnAppointmentTimerTick;
            _timerAppointment.Start();
        }

        private async void OnAppointmentTimerTick(object sender, EventArgs e)
        {
            await LoadTodayAppointmentsAsync();
        }


        // �������� ��� ����������� ���������� � ������ �� �������
        public string AppointmentTodayOne
        {
            get => _appointmentTodayOne;
            set
            {
                _appointmentTodayOne = value;
                OnPropertyChanged();
            }
        }

        public string AppointmentTodayTwo
        {
            get => _appointmentTodayTwo;
            set
            {
                _appointmentTodayTwo = value;
                OnPropertyChanged();
            }
        }


        // ����� ��� ���������� ������ AppointmentToday
        private void UpdateAppointmentToday()
        {
            if (TodayAppointments != null && TodayAppointments.Any())
            {
                var sortedAppointments = TodayAppointments.OrderBy(a => a.DateTime).Take(2).ToList();

                if (sortedAppointments.Count >= 1)
                {
                    var firstAppointment = sortedAppointments[0];
                    AppointmentTodayOne = $"{firstAppointment.DateTime:t} {firstAppointment.Doctor.LastName} {firstAppointment.Doctor.FirstName[0]}. {firstAppointment.Doctor.Patronymic?[0]}.";
                }
                else
                {
                    AppointmentTodayOne = "��� ��������� �������.";
                }

                if (sortedAppointments.Count == 2)
                {
                    var secondAppointment = sortedAppointments[1];
                    AppointmentTodayTwo = $"{secondAppointment.DateTime:t} {secondAppointment.Doctor.LastName} {secondAppointment.Doctor.FirstName[0]}. {secondAppointment.Doctor.Patronymic?[0]}.";
                }
                else
                {
                    AppointmentTodayTwo = "��� ���������� ������.";
                }
            }
            else
            {
                AppointmentTodayOne = "��� ��������� �������.";
                AppointmentTodayTwo = string.Empty;
            }

            OnPropertyChanged(nameof(AppointmentTodayOne));
            OnPropertyChanged(nameof(AppointmentTodayTwo));
        }

        // �������� ��� ������ �������
        public ObservableCollection<AppointmentModel> TodayAppointments
        {
            get => _todayAppointments;
            set
            {
                _todayAppointments = value;
                OnPropertyChanged();
            }
        }

        // ����������� ����� ��� �������� ������� �� �������
        private async Task LoadTodayAppointmentsAsync()
        {
            IsLoading = true;
            try
            {
                using (var context = new ClinicDbContext())
                {
                    var repository = new AppointmentRepository(context);
                    var appointments = await repository.GetAppointmentsForPatientTodayAsync(PatientId);
                    TodayAppointments = new ObservableCollection<AppointmentModel>(appointments);
                    UpdateAppointmentToday();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� �������� ������: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ���������� �������� ��� ��������������� ���������� �������
        public string CurrentTime => DateTime.Now.ToString("HH:mm");
        public string CurrentDate => DateTime.Now.ToString("dd.MM.yyyy");
        public string CurrentDayOfWeek => DateTime.Now.ToString("dddd");

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