using System.Windows;

using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardMenuVM : BaseViewModel
{
    private MedCardVM _parentVM;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ClinicDbContext _context = new ClinicDbContext();
    
    public MedCardMenuVM(MedCardVM parentVM)
    {
        _parentVM = parentVM;
        _appointmentRepository = new AppointmentRepository(_context);
        
        DoctorId = 1; // Здесь можно установить текущий ID врача, если он задаётся динамически
        CurrentPatientId = 1; // Текущий ID пациента
        
        InitializeCommands();
    }
    
    private void InitializeCommands()
    {
        ShowSearchCommand = new RelayCommand(ShowSearch);
        ShowInfoMedCardCommand = new RelayCommand(async o => await CheckPatientStatusAndShowInfoAsync(o));
    }
    public ICommand ShowSearchCommand { get; private set; }
    public ICommand ShowInfoMedCardCommand { get; private set; }
    
    private void ShowSearch(object parameter)
    {
        _parentVM.ShowSearch(parameter);
    }
    
    private async Task CheckPatientStatusAndShowInfoAsync(object obj)
    {
        try
        {
            bool isInProgress = await _appointmentRepository.IsPatientInProgressAsync(DoctorId, CurrentPatientId);
            if (isInProgress)
            {
                _parentVM.ShowInfoMedCard(obj);
            }
            else
            {
                MessageBox.Show("Пациент со статусом InProgress не найден.", 
                    "Предупреждение", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка: {ex.Message}", 
                "Ошибка", 
                MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }
    }
    
    
    private int _doctorId; // ID текущего врача
    public int DoctorId
    {
        get => _doctorId;
        set
        {
            _doctorId = value;
            OnPropertyChanged();
        }
    }

    private int _currentPatientId; // ID текущего пациента
    public int CurrentPatientId
    {
        get => _currentPatientId;
        set
        {
            _currentPatientId = value;
            OnPropertyChanged();
        }
    }

}