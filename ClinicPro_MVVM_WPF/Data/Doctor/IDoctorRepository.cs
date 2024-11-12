using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Data.Doctor;

public interface IDoctorRepository
{
    Task<IEnumerable<DoctorModel>> GetAllDoctorsAsync();
    Task<DoctorModel?> GetDoctorByIdAsync(int? doctorId);
    Task AddDoctorAsync(DoctorModel doctor);
    Task UpdateDoctorAsync(DoctorModel doctor);
    Task DeleteDoctorAsync(int doctorId);
    
    
    
}