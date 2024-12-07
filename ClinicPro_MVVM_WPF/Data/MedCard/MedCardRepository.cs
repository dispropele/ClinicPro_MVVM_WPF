using ClinicPro_MVVM_WPF.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.MedCard;

public class MedCardRepository : IMedCardRepository
{
    private readonly ClinicDbContext _context;

    public MedCardRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedCardModel>> GetAllMedCardAsync()
    {
        return await _context.MedCard.Include(md => md.Patient).ToListAsync();
    }
    
    //добавил исключение null
    public async Task<MedCardModel?> GetMedCardByIdAsync(int? id)
    {
        return await _context.MedCard.Include(md => md.Patient)
                                     .FirstOrDefaultAsync(md => md.medcardId == id);
    }

    public async Task AddMedCardAsync(MedCardModel medCard)
    {
        await _context.MedCard.AddAsync(medCard);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMedCardAsync(MedCardModel medCard)
    {
        _context.MedCard.Update(medCard);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMedCardAsync(int id)
    {
        var medCard = await _context.MedCard.FindAsync(id);
        if (medCard != null)
        {
            _context.MedCard.Remove(medCard);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<IEnumerable<MedCardModel>> GetMedCardByFamilyAsync(string lastName)
    {
        return await _context.MedCard.Include(md => md.Patient)
                                     .Where(md => md.Patient.firstName.Contains(lastName))
                                     .ToListAsync();
    }

    public async Task<IEnumerable<MedCardModel>> GetMedCardByDateOfBirthAsync(string dateOfBirth)
    {
        if (DateTime.TryParse(dateOfBirth, out var parsedDate))
        {
            int parsedYear = parsedDate.Year; // Оптимизация: вычисляем год только один раз
            return await _context.MedCard
                .Include(md => md.Patient)
                .Where(md => md.Patient.dateOfBirth.GetValueOrDefault().Year == parsedYear) //GetValueOrDefault() вернет значение по умолчанию для DateTime, если оно равно null. В данном случае, это будет DateTime.MinValue, и у него есть Year.
                .ToListAsync();
        }
        else
        {
            return Enumerable.Empty<MedCardModel>();
        }
    }

    public async Task<MedCardModel> GetMedCardByPatientAsync(int patientId)
    {
        return await _context.MedCard.Include(md => md.Patient)
                                     .FirstOrDefaultAsync(md => md.patientId == patientId);
    }
    
}