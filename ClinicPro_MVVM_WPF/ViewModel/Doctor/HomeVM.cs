

using System.Windows.Threading;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Service;
using ClinicPro_MVVM_WPF.View.Doctor;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class HomeVM : Utils.BaseViewModel
    {
        private string _appointmentToday;
        private DispatcherTimer _timer;
        private readonly AppointmentService _appointmentService = new AppointmentService();
        
        private static int ID = 2;
        
        
        public HomeVM()
        {
            // AppointmentTime = new DateTime(2024, 10, 22, 10, 00, 00).ToString();
            // LastName = "Иванов";
            // FirstName = "Иван";
            // Patronymic = "Иванович";
            
            LoadAppointmentTodayAsync();
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += UpdateDateTime;
            _timer.Start();
            
            UpdateDateTime(null, null); 
            
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd.MM.yyyy");
            CurrentDayOfWeek = DateTime.Now.ToString("dddd"); // Полное название дня недели
            OnPropertyChanged();
        }

        private async void LoadAppointmentTodayAsync()
        {
            AppointmentToday = await GetAppointmentTodayAsync();
        }
        
        public string AppointmentToday
        {
            get => _appointmentToday;
            set
            {
                _appointmentToday = value;
                OnPropertyChanged(nameof(AppointmentToday));
            }
        }
        
        private async Task<string> GetAppointmentTodayAsync()
        {
            // Логика метода получения данных
            try
            {
                List<AppointmentModel> appointments = await _appointmentService.getTodayAppointment(2);
                if (appointments == null || appointments.Count == 0)
                {
                    return "Нет записей на сегодня";
                }

                AppointmentModel todayAppointment = appointments[0];
                string todayTime = todayAppointment.dateTime.ToString("t");
                string todayFio = $"{todayAppointment.patient.lastName} {todayAppointment.patient.firstName[0]}. {todayAppointment.patient.patronymic?[0] ?? '-'}.";

                return todayTime + " " + todayFio;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return "Ошибка при получении данных";
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        private string _currentDate;
        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }

        private string _currentDayOfWeek;
        public string CurrentDayOfWeek
        {
            get => _currentDayOfWeek;
            set
            {
                _currentDayOfWeek = value;
                OnPropertyChanged();
            }
        }
    }
}
