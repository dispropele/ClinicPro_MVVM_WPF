using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Data.Patient;

public interface IPatientRepository
{
    Task<List<PatientModel>> GetAllPatientsAsync();
    Task<PatientModel?> GetPatientByIdAsync(int? patientId);
    Task AddPatientAsync(PatientModel patient);
    Task UpdatePatientAsync(PatientModel patient);
    Task DeletePatientAsync(int patientId);
}