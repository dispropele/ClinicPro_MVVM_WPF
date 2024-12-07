using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Data.MedCard;

public interface IMedCardRepository
{
    Task<IEnumerable<MedCardModel>> GetAllMedCardAsync();
    Task<MedCardModel?> GetMedCardByIdAsync(int? id);
    Task AddMedCardAsync(MedCardModel medCard);
    Task UpdateMedCardAsync(MedCardModel medCard);
    Task DeleteMedCardAsync(int id);

    Task<IEnumerable<MedCardModel>> GetMedCardByFamilyAsync(string lastName);
    Task<IEnumerable<MedCardModel>> GetMedCardByDateOfBirthAsync(string dateOfBirth);
    Task<MedCardModel> GetMedCardByPatientAsync(int patientId);
}