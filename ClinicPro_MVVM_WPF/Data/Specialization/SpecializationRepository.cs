using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.Specialization;

public class SpecializationRepository
{
    private readonly ClinicDbContext _context;

    public SpecializationRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SpecializationModel>> GetAllSpecializationsAsync()
    {
        return await _context.Specialization.ToListAsync();
    }

    public async Task<IEnumerable<SpecializationModel>> GetFilteredSpecializationsAsync(string specializationName)
    {
        return await _context.Specialization
                                            .Where(s => s.NameSpecialization.Contains(specializationName))
                                            .ToListAsync();
    }
    
}