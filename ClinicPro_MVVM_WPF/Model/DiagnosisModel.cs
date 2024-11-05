
namespace ClinicPro_MVVM_WPF.Model
{
    public class DiagnosisModel
    {
        public int diagnosisId { get; set; }
        
        public string code { get; set; } // Код диагноза (например, МКБ-10)
        
        public string name { get; set; }
        
        public string description { get; set; }
        
    }
}
