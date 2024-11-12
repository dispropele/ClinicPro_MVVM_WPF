using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class HomeVM : Utils.BaseViewModel
    {
        // private static ClinicDbContext _context = new ClinicDbContext();
        // private readonly IAppointmentRepository _appointmentRepository = new AppointmentRepository(_context);
        private ObservableCollection<AppointmentModel> _todayAppointments;

        private string _appointmentTodayOne;
        private string _appointmentTodayTwo;
        private DispatcherTimer _timerAppointment;
        private static int ID = 2;

        public HomeVM()
        {
            InitializeTimers(); // Вызов метода инициализации таймеров
            Task.Run(() => LoadTodayAppointmentsAsync()); // Инициализация данных при загрузке ViewModel
        }


        private void InitializeTimers()
        {
            // Инициализация таймера для обновления данных
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


        // Свойство для отображения информации о записи на сегодня
        public string AppointmentTodayOne
        {
            get => _appointmentTodayOne;
            set
            {
                _appointmentTodayOne = value;
                OnPropertyChanged(nameof(AppointmentTodayOne));
            }
        }

        public string AppointmentTodayTwo
        {
            get => _appointmentTodayTwo;
            set
            {
                _appointmentTodayTwo = value;
                OnPropertyChanged(nameof(AppointmentTodayTwo));
            }
        }


        // Метод для обновления строки AppointmentToday
        private void UpdateAppointmentToday()
        {
            if (TodayAppointments != null && TodayAppointments.Any())
            {
                var sortedAppointments = TodayAppointments.OrderBy(a => a.DateTime).Take(2).ToList();

                if (sortedAppointments.Count >= 1)
                {
                    var firstAppointment = sortedAppointments[0];
                    AppointmentTodayOne = $"{firstAppointment.DateTime:t} {firstAppointment.Patient.LastName} {firstAppointment.Patient.FirstName[0]}. {firstAppointment.Patient.Patronymic[0]}.";
                }
                else
                {
                    AppointmentTodayOne = "Нет ближайших записей.";
                }

                if (sortedAppointments.Count == 2)
                {
                    var secondAppointment = sortedAppointments[1];
                    AppointmentTodayTwo = $"{secondAppointment.DateTime:t} {secondAppointment.Patient.LastName} {secondAppointment.Patient.FirstName[0]}. {secondAppointment.Patient.Patronymic[0]}.";
                }
                else
                {
                    AppointmentTodayTwo = "Нет следующей записи.";
                }
            }
            else
            {
                AppointmentTodayOne = "Нет ближайших записей.";
                AppointmentTodayTwo = string.Empty;
            }

            OnPropertyChanged(nameof(AppointmentTodayOne));
            OnPropertyChanged(nameof(AppointmentTodayTwo));
        }

        // Свойство для списка записей
        public ObservableCollection<AppointmentModel> TodayAppointments
        {
            get => _todayAppointments;
            set
            {
                _todayAppointments = value;
                OnPropertyChanged(nameof(TodayAppointments));
                OnPropertyChanged(nameof(CountAppointment));
            }
        }

        // Асинхронный метод для загрузки записей на сегодня
        private async Task LoadTodayAppointmentsAsync()
        {
            IsLoading = true;
            try
            {
                using (var context = new ClinicDbContext())
                {
                    var repository = new AppointmentRepository(context);
                    var appointments = await repository.GetAppointmentsForDoctorTodayAsync(ID);
                    TodayAppointments = new ObservableCollection<AppointmentModel>(appointments);
                    UpdateAppointmentToday();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Используем свойство для автоматического обновления времени
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
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public int CountAppointment => TodayAppointments?.Count ?? 0;



    }
}
