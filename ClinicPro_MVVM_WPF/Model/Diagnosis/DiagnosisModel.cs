using ClinicPro_MVVM_WPF.Model.MedCardRecord;


namespace ClinicPro_MVVM_WPF.Model.Diagnosis
{
    public class DiagnosisModel
    {
        public int DiagnosisId { get; set; }
        public string Code { get; set; } // Код диагноза (например, МКБ-10)
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MedRecordModel> MedRecords { get; set; } = new HashSet<MedRecordModel>(); // Навигационное свойство для записей
    }
}
