using ClinicPro_MVVM_WPF.Model.Appointment;
using ClinicPro_MVVM_WPF.Model.Patient;
using System.Windows.Threading;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class HomeVM : Utils.BaseViewModel
    {
        private readonly AppointmentModel _appointmentModel;

        private DispatcherTimer _timer;

        public HomeVM()
        {
            _appointmentModel = new AppointmentModel();
            AppointmentTime = new DateTime(2024, 10, 22, 10, 00, 00).ToString();
            LastName = "Иванов";
            FirstName = "Иван";
            Patronymic = "Иванович";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += UpdateDateTime;
            _timer.Start();

            UpdateDateTime(null, null);
        }


        public string Appointment
        {
            get
            {
                return $"- {AppointmentTime} {LastName} {FirstNameChar}.{PatronymicChar}.";
            }
        }


        private void UpdateDateTime(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm");
            CurrentDate = DateTime.Now.ToString("dd.MM.yyyy");
            CurrentDayOfWeek = DateTime.Now.ToString("dddd"); // Полное название дня недели
        }

        public string AppointmentTime
        {
            get => _appointmentModel.DateTime.ToString("t");
            set
            {
                if (_appointmentModel != null && DateTime.TryParse(value, out DateTime parsedTime))
                {
                    _appointmentModel.DateTime = _appointmentModel.DateTime.Date.Add(parsedTime.TimeOfDay);
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => _appointmentModel.Patient.LastName; // проверяем на null
            set
            {
                if (_appointmentModel != null)
                {
                    _appointmentModel.Patient.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirstName
        {
            get => _appointmentModel.Patient.FirstName; // полное имя
            set
            {
                if (_appointmentModel != null)
                {
                    _appointmentModel.Patient.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirstNameChar
        {
            get => !string.IsNullOrEmpty(_appointmentModel.Patient.FirstName)
                    ? _appointmentModel.Patient.FirstName[0].ToString()
                    : string.Empty; // проверяем, что имя не пустое
        }

        public string Patronymic
        {
            get => _appointmentModel.Patient.Patronymic ?? string.Empty; // полное отчество
            set
            {
                if (_appointmentModel != null)
                {
                    _appointmentModel.Patient.Patronymic = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PatronymicChar
        {
            get => !string.IsNullOrEmpty(_appointmentModel.Patient.Patronymic)
                    ? _appointmentModel.Patient.Patronymic[0].ToString()
                    : string.Empty; // проверяем, что отчество не пустое
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
