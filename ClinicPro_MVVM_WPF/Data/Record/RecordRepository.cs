using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.Record;

public class RecordRepository
{
    private readonly ClinicDbContext _context;

    public RecordRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<MedRecordModel?> GetRecordByIdAsync(int recordId)
    {
        return await _context.Med_Record.Include(r => r.Doctor)
                                        .Include(r => r.Diagnosis)
                                        .FirstOrDefaultAsync(r => r.RecordId == recordId);
    }

    public async Task<IEnumerable<MedRecordModel>> GetRecordsAsync(int medCardId)
    {
        return await _context.Med_Record
            .Include(r => r.MedCard)
            .Include(r => r.Diagnosis)
            .Include(r => r.Doctor)
            .Include(r => r.MedCard.Patient)
            .Where(r => r.MedCardId == medCardId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedRecordModel>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Med_Record.Where(r => r.DateTime >= startDate && r.DateTime <= endDate)
                                        .ToListAsync();
    }

    public async Task AddRecordAsync(MedRecordModel record)
    {
        await _context.Med_Record.AddAsync(record);
        await _context.SaveChangesAsync();
    }
    
}