using ClinicPro_MVVM_WPF.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicPro_MVVM_WPF.Data.Doctor;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;

    public DoctorRepository(ClinicDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<DoctorModel>> GetAllDoctorsAsync()
    {
        return await _context.Doctor.Include(d => d.Specialization).ToListAsync();
    }

    public async Task<DoctorModel?> GetDoctorByIdAsync(int? doctorId)
    {
        return await _context.Doctor
            .Include(d => d.Specialization)
            .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
    }

    public async Task AddDoctorAsync(DoctorModel doctor)
    {
        await _context.Doctor.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDoctorAsync(DoctorModel doctor)
    {
        _context.Doctor.Update(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDoctorAsync(int doctorId)
    {
        var doctor = await _context.Doctor.FindAsync(doctorId);
        if (doctor != null)
        {
            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }
}