
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
        return await _context.Patient
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.patientId == patientId);
    }

    public async Task<PatientModel?> GetPatientByUserIdAsync(int userId)
    {
        return await _context.Patient
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.userId == userId);
    }

    public async Task AddPatientAsync(PatientModel patient)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            //добавляем пациента
            await _context.Patient.AddAsync(patient);
            await _context.SaveChangesAsync();
            
            //создаем медкарту
            var medCard = new MedCardModel()
            {
                patientId = patient.patientId,
                startDate = DateTime.Now
            };
            
            //добавляем медкарту
            await _context.MedCard.AddAsync(medCard);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
            
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
     
        }
        
        
        
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
    

    public async Task<int> GetPatientIdByUserIdAsync(int userId)
    {
        return await _context.Patient.Where(p => p.userId == userId)
                                     .Select(p => p.patientId)
                                     .FirstOrDefaultAsync();
    }


}