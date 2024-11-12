using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.Patient;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicDbContext _context;
    
    public PatientRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<List<PatientModel>> GetAllPatientsAsync()
    {
        return await _context.Patient.ToListAsync();
    }

    public async Task<PatientModel?> GetPatientByIdAsync(int? patientId)
    {
        return await _context.Patient.FindAsync(patientId);
    }

    public async Task AddPatientAsync(PatientModel patient)
    {
        await _context.Patient.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePatientAsync(PatientModel patient)
    {
        _context.Patient.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePatientAsync(int patientId)
    {
        var patient = await _context.Patient.FindAsync(patientId);
        if (patient != null)
        {
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
    
    
}