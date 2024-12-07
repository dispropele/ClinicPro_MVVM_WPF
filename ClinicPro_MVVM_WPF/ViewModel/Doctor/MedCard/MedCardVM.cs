
using ClinicPro_MVVM_WPF.Utils;

using System.Windows.Input;


namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

    public class MedCardVM : BaseViewModel
    {
        private object _currentMedCardView;

        public object CurrentMedCardView
        {
            get => _currentMedCardView;
            set
            {
                _currentMedCardView = value;
                OnPropertyChanged();
            }
        }

        // Команда для переключения на MedCardSearch
        public ICommand ShowSearchCommand { get; set; }
        public ICommand ShowMenuMedCardCommand { get; set; }
        public ICommand ShowInfoMedCardCommand { get; set; }
        
        private MedCardMenuVM MedCardMenuVM { get; set; }
        private MedCardSearchVM MedCardSearchVM { get; set; }
        public MedCardInfoVM MedCardInfoVM { get; set; }
        
        public void ShowSearch(object obj) => CurrentMedCardView = MedCardSearchVM;
        public void ShowMenuMedCard(object obj) => CurrentMedCardView = MedCardMenuVM;
        public void ShowInfoMedCard(object obj) => CurrentMedCardView = MedCardInfoVM;
        
        public int DoctorId { get; set; }
        
        public MedCardVM(int doctorId)
        {
            MedCardSearchVM = new MedCardSearchVM(this);
            MedCardMenuVM = new MedCardMenuVM(this, doctorId);
            
            DoctorId = doctorId;
            
            CurrentMedCardView = MedCardMenuVM;
            
            ShowSearchCommand = new RelayCommand(ShowSearch);
            ShowMenuMedCardCommand = new RelayCommand(ShowMenuMedCard);
            ShowInfoMedCardCommand = new RelayCommand(ShowInfoMedCard);
    
        }
        
        
 
    }

