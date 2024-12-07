using ClinicPro_MVVM_WPF.Utils;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.View.Doctor;
using ClinicPro_MVVM_WPF.View.Doctor.MedCard;
using ClinicPro_MVVM_WPF.ViewModel.Doctor;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel
{
    public class DoctorNavigationVM : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand MedCardCommand { get; set; }
        public ICommand AppointmentCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        
        private MedCard MedCardView { get; set; }
        private Appointment AppointmentView { get; set; }
        
        private void Home(object obj) => CurrentView = new Home(DoctorId);
        private void MedCard(object obj) => CurrentView = MedCardView;
        private void Appointment(object obj) => CurrentView = AppointmentView;
        private void Account(object obj) => CurrentView = new Account(DoctorId);

        public DoctorNavigationVM(int doctorId)
        {
            DoctorId = doctorId;
            Console.WriteLine(DoctorId);
            
            MedCardView = new MedCard(doctorId); 
            AppointmentView = new Appointment(doctorId);
            
            HomeCommand = new RelayCommand(Home);
            MedCardCommand = new RelayCommand(MedCard);
            AppointmentCommand = new RelayCommand(Appointment);
            AccountCommand = new RelayCommand(Account);
            
            CurrentView = new Home(DoctorId);
        }
        
        private int DoctorId { get; set; }
    }
}
