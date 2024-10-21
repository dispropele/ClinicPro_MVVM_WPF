using ClinicPro_MVVM_WPF.Model.Appointment;
using ClinicPro_MVVM_WPF.Model.Patient;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class HomeVM : Utils.BaseViewModel
    {
        private readonly AppointmentModel _appointmentModel;

        public HomeVM()
        {
            _appointmentModel = new AppointmentModel();
            AppointmentTime = new TimeSpan(10, 00, 00);
            LastName = "Иванов";
            FirstName = "Иван";
            Patronymic = "Иванович";
        }

        public TimeSpan AppointmentTime
        {
            get => _appointmentModel.Time; // проверяем на null
            set
            {
                if (_appointmentModel != null)
                {
                    _appointmentModel.Time = value;
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
                    ? _appointmentModel.Patient.FirstName[0].ToString() + "."
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
                    ? _appointmentModel.Patient.Patronymic[0].ToString() + "."
                    : string.Empty; // проверяем, что отчество не пустое
        }
    }
}
