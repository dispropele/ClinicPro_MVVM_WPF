using System.Collections.ObjectModel;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Appointment;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Admin.Doctors;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class DoctorsListVM : BaseViewModel
{
    private readonly ClinicDbContext _context;
    private readonly DoctorRepository _repDoctor;
    
    public ICommand ViewDoctorDataCommand { get; set; }
    public ICommand AddNewDoctorCommand { get; set; }
    public ICommand DeleteDoctorDataCommand { get; set; }
    
    private ObservableCollection<DoctorModel> _doctors;
    public ObservableCollection<DoctorModel> Doctors 
    {
        get => _doctors;
        set 
        { 
            _doctors = value;
            OnPropertyChanged();
        }
    }
    
    private bool isLoading = true;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            OnPropertyChanged();
        }
    }

    private DoctorsVM _parentVm;
    
    public DoctorsListVM(DoctorsVM parentVm)
    {
        _parentVm = parentVm;
        
        _context = new ClinicDbContext();
        _repDoctor = new DoctorRepository(_context);
        
        Doctors = new ObservableCollection<DoctorModel>();
        
        ViewDoctorDataCommand = new RelayCommand(o => ViewDoctorData(o));
        AddNewDoctorCommand = new RelayCommand(o => AddNewDoctor(o));
        DeleteDoctorDataCommand = new RelayCommand(o => DeleteDoctor(o));
        
        LoadDoctors();
    }

    private async Task LoadDoctors()
    {
        IsLoading = true;
        try
        {
            var doctors = await _repDoctor.GetAllDoctorsAsync();
            
            if (doctors != null)
            {
                AppointmentRepository rep = new AppointmentRepository(_context);
                Doctors.Clear();
                foreach (var doctor in doctors)
                {
                    await doctor.CalculateAppointmentCount(rep);
                    Doctors.Add(doctor);
                }
                Console.WriteLine($"Loaded {Doctors.Count} doctors");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки: " + ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ViewDoctorData(object obj)
    {
        if (obj is int doctorId)
        {
            _parentVm.CurrentView = new ViewDoctorData(_parentVm, doctorId);
        }
            
    }

    private void AddNewDoctor(object obj) => _parentVm.CurrentView = new AddDoctorView(_parentVm);

    private void DeleteDoctor(object obj)
    {
        //Логика удаления врача, показ сообщения предупреждения
    }
    
    
}