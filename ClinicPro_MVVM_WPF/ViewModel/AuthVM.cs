using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel
{
    public class AuthVM : BaseViewModel
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
        
        private ClinicDbContext _context;
        private IPatientRepository _patientRepository;
        private UserRepository _userRepository;
        
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        
        private LoginVM LoginVM { get; set; }
        private RegistrationVM RegistrationVM { get; set; }

        public void ShowLogin(object view) => CurrentView = LoginVM;
        public void ShowRegistration(object view) => CurrentView = RegistrationVM;
        
        public AuthVM()
        {
            try
            {
                _context = new ClinicDbContext();
                _patientRepository = new PatientRepository(_context);
                _userRepository = new UserRepository(_context);
            
                RegistrationVM = new RegistrationVM(this, _userRepository, _patientRepository);
                LoginVM = new LoginVM(this);
            
                CurrentView = LoginVM;
            
                LoginCommand = new RelayCommand(ShowLogin);
                RegisterCommand = new RelayCommand(ShowRegistration);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка подключения: "+e.Message);
            }
        }
        
        
        
    }
}
