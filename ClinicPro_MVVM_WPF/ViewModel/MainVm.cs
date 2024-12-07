using System.IO;
using System.Windows;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Data.Patient;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View;
using Newtonsoft.Json;

namespace ClinicPro_MVVM_WPF.ViewModel;

public class MainVm : BaseViewModel
{
    private readonly DoctorRepository _doctorRep;
    private readonly PatientRepository _patientRep;

    private int Id { get; set; }
    private int UserId { get; set; }
    
    private string JsonPath = "login_data.json";
    
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

    public MainVm(ClinicDbContext context)
    {
        _doctorRep = new DoctorRepository(context);
        _patientRep = new PatientRepository(context);
        
        LoadUserId();
        InitializeDataContext();
    }
    
    private async Task InitializeDataContext()
    {
        switch (GetUserRole())
        {
            case "doctor":
            { 
                await LoadIdDoctor(); //загружаем id доктора
                CurrentView = new DoctorView(Id);
                break;
            }
            case "patient":
            {
                await LoadIdPatient(); //загружаем id пациента
                CurrentView = new PatientView(Id);
                break;
            }
            case "admin":
            {
                CurrentView = new AdminView(); 
                break;
            }
        }
    }
    
    private async Task LoadIdDoctor()
    {
        Id = await _doctorRep.GetDoctorIdByUserId(UserId);
    }

    private async Task LoadIdPatient()
    {
        Id = await _patientRep.GetPatientIdByUserIdAsync(UserId);
    }
    
    private void LoadUserId()
    {
        if (File.Exists(JsonPath))
        {
            string json = File.ReadAllText(JsonPath);
            try
            {
                var loginData = JsonConvert.DeserializeObject<LoginData>(json);
                UserId = loginData.Id;
            }
            catch (JsonException)
            {
                MessageBox.Show($"Ошибка десериализации: {JsonPath}");
            }
        }
    }
    
    private string GetUserRole()
    {
        if (File.Exists(JsonPath))
        {
            string json = File.ReadAllText(JsonPath);
            try
            {
                var loginData = JsonConvert.DeserializeObject<LoginData>(json);
                return loginData.Role;
            }
            catch (JsonException)
            {
                MessageBox.Show($"Ошибка десериализации: {JsonPath}");
            }
        }
        return string.Empty;
    }
    
    // Класс для хранения данных входа
    private class LoginData
    {
        public int Id { get; set; } //хранение id пользователя(врач/пациент)
        public string Login { get; set; }
        public string Role { get; set; }
    }
    
}