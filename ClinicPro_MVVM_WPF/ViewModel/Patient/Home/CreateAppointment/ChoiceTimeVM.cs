using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Appointment;
using ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Home.CreateAppointment;

public class ChoiceTimeVM: BaseViewModel
{
    private HomeVM _parentVm;
    private int PatientId { get; set; }
    private int DoctorId { get; set; }

    private readonly ClinicDbContext _context;
    private AppointmentRepository _repAppointment;
    
    public ICommand BackToCommand { get; set; }
    public ICommand CreateAppointmentCommand { get; set; }
    
    public ChoiceTimeVM(HomeVM parentVm, int patientId, int doctorId)
    {
        _parentVm = parentVm;
        PatientId = patientId;
        DoctorId = doctorId;

        _context = new ClinicDbContext();
        _repAppointment = new AppointmentRepository(_context);

        BackToCommand = new RelayCommand(BackTo);
        CreateAppointmentCommand = new RelayCommand(async o => await CreateAppointment(o));
    }
    
    

    private DateTime _date = DateTime.Now.Date;
    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }
    
    private TimeSpan _time;
    public TimeSpan Time
    {
        get => _time;
        set
        {
            _time = value;
            OnPropertyChanged();
        }
    }

    private string _textReason = string.Empty;
    public string TextReason
    {
        get => _textReason;
        set
        {
            _textReason = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    
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
    
    private async Task CreateAppointment(object obj)
    {
        IsLoading = true;
        try
        {
            var dateTime = Date + Time;
            Console.WriteLine("������� �����: " + dateTime);

            if (Date == null || Date < DateTime.Today)
            {
                ErrorMessage = "Некорректная дата!";
                GetMessageBox();
                return;
            }

            if (Time == null || Time.Hours < 8 || Time.Hours > 18)
            {
                ErrorMessage = "Некорректное время!";
                GetMessageBox();
                return;
            }

            var appointments = await _repAppointment.GetAppointmentsForPatientTodayAsync(dateTime);
            if (appointments != null && appointments.Any())
            {
                ErrorMessage = "На это время уже есть запись!";
                GetMessageBox();
                return;
            }
            
            Console.WriteLine($"{dateTime} || {DoctorId} || {PatientId} || {TextReason}");
            var appointment = new AppointmentModel()
            {
                DateTime = dateTime,
                PatientId = PatientId,
                DoctorId = DoctorId,
                Status = "Scheduled",
                Reason = TextReason
            };
            
            await _repAppointment.AddAppointmentAsync(appointment);
            GetMessageBox("Успешная запись на прием!");

            _parentVm.CurrentView = new AppointmentParent(PatientId);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            GetMessageBox(e.Message);
        }
        finally
        {
            IsLoading = false;
        }
        
    }

    private void GetMessageBox()
    {
        MessageBox.Show(ErrorMessage, "Ошибка!");
    }

    private void GetMessageBox(string message)
    {
        MessageBox.Show(message, "Ошибка!");
    }
    
    private void BackTo(object obj) => _parentVm.CurrentView = new ChoiceDoctors(_parentVm, PatientId);
}