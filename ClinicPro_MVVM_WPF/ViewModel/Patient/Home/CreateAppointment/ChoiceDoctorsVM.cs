using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Data.Specialization;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.Home.CreateAppointment;

public class ChoiceDoctorsVM : BaseViewModel
{
    private HomeVM _parentVm;
    private int PatientId { get; set; }
    
    public ICommand BackToCommand { get; set; }
    public ICommand SelectDoctorCommand { get; set; }
    public ICommand SearchDoctorBySpecializationCommand { get; set; }
    
    public ObservableCollection<string> Specialization { get; set; }
    public ObservableCollection<DoctorModel> Doctors { get; set; }
    public ObservableCollection<DoctorModel> FilteredDoctors { get; set; }

    private readonly ClinicDbContext _context;
    private DoctorRepository _repDoctor;
    private SpecializationRepository _repSpec;
    
    public ChoiceDoctorsVM(HomeVM parentVm, int patientId)
    {
        _parentVm = parentVm;
        PatientId = patientId;

        BackToCommand = new RelayCommand(BackTo);
        SelectDoctorCommand = new RelayCommand(SelectDoctor);
        SearchDoctorBySpecializationCommand = new RelayCommand(async o => await SearchDoctorBySpecialization(o));

        Specialization = new ObservableCollection<string>();
        Doctors = new ObservableCollection<DoctorModel>();
        FilteredDoctors = new ObservableCollection<DoctorModel>();
        
        _context = new ClinicDbContext();
        _repDoctor = new DoctorRepository(_context);
        _repSpec = new SpecializationRepository(_context);
        
        Task.Run(async () => await LoadSpecializationAsync());
    }

    private async Task LoadDoctorsAsync()
    {
        IsLoading = true;
        try
        {
            var doctors = await _repDoctor.GetAllDoctorsAsync();
            if (doctors == null) throw new Exception("?????? ?? ??????? ? ???? ??????");

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Doctors.Clear();
                foreach (var doctor in doctors)
                {
                    doctor.Photo ??= GetDefaultDoctorImageBytes();
                    Doctors.Add(doctor);
                }
                FilteredDoctors = Doctors;
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine("?????? ???????? ?????: " + ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadSpecializationAsync()
    {
        IsLoading = true;
        try
        {
            var specializations = await _repSpec.GetAllSpecializationsAsync();
            if (specializations == null) throw new Exception("?????????????? ?? ??????? ? ???? ??????");

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Specialization.Clear();
                foreach (var spec in specializations)
                {
                    Specialization.Add(spec.NameSpecialization);
                }
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine("?????? ???????? ??????????????: " + ex.Message);
        }
        finally
        {
            await LoadDoctorsAsync();
        }
    }
    
    private byte[] GetDefaultDoctorImageBytes()
    {
        try
        {
            // ??????? Uri ??? ??????, ???????????, ??? ??? ???????? ??? Resource
            var uri = new Uri("pack://application:,,,/Resources/doctor.png"); 

            // ????????? ??????????? ?? Uri
            var bitmap = new BitmapImage(uri);

            // ???????? ??????????? ? PNG ? ???????? ?????? ??????
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
        catch (Exception ex)
        {
            // ????????? ?????? (????????, ???????????)
            Console.WriteLine($"?????? ??? ???????? ??????? ?????? ?????: {ex.Message}");
            return null; 
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

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }
    
    private void BackTo(object obj) => _parentVm.Home(obj);

    private void SelectDoctor(object obj)
    {
        if (obj is int doctorId)
        {
            _parentVm.CurrentView = new ChoiceTime(_parentVm, PatientId, doctorId);
        }
        else
        {
            MessageBox.Show("?????? ???????? ? ?????");
        }
    }
    
    private async Task SearchDoctorBySpecialization(object obj)
    {
        try
        {
            if (SearchText == null) throw new Exception("?????? ???? ??????");
            
            var doctors = await _repDoctor.GetDoctorsBySpecialization(SearchText);
            if (doctors == null) throw new Exception("?? ??????? ????? ? ???? ?????? ? ?????? ??????????????");
            
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                FilteredDoctors.Clear();
                foreach (var doctor in doctors)
                {
                    FilteredDoctors.Add(doctor);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("?????? ?????????? ?? ?????????????: "+ex.Message);
        }
    }
    
}