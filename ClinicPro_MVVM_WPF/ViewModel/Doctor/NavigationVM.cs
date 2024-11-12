using ClinicPro_MVVM_WPF.Utils;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public class NavigationVM : BaseViewModel
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
        public ICommand ChatCommand { get; set; }
        public ICommand PharmaCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        
        private void Home(object obj) => CurrentView = new HomeVM();
        private void MedCard(object obj) => CurrentView = new MedCardVM();
        private void Appointment(object obj) => CurrentView = new AppointmentVM();
        private void Chat(object obj) => CurrentView = new ChatVM();
        private void Pharma(object obj) => CurrentView = new PharmaVM();
        private void Settings(object obj) => CurrentView = new SettingsVM();
        private void Account(object obj) => CurrentView = new AccountVM();

        public NavigationVM()
        {
            
            HomeCommand = new RelayCommand(Home);
            MedCardCommand = new RelayCommand(MedCard);
            AppointmentCommand = new RelayCommand(Appointment);
            ChatCommand = new RelayCommand(Chat);
            PharmaCommand = new RelayCommand(Pharma);
            SettingsCommand = new RelayCommand(Settings);
            AccountCommand = new RelayCommand(Account);
            
            CurrentView = new HomeVM();
        }
    }
}
