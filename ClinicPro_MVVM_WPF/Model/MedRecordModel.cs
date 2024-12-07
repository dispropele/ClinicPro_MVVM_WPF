
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class MedRecordModel
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("record_id")]
        public int RecordId { get; set; }
        
        [Required]
        [ForeignKey("MedCard")]
        [Column("medcard_id")]
        public int MedCardId { get; set; }
        
        [Required]
        [ForeignKey("Doctor")]
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        
        [Required]
        [Column("date_time")]
        public DateTime DateTime { get; set; }
        
        [Column("complaints")]
        public string Complaints { get; set; } = string.Empty;
        
        [Column("examination")]
        public string? Examination { get; set; } = string.Empty;
        
        [Column("recommendation")]
        public string? Recommendation { get; set; } = string.Empty;
        
        [Column("treatment")]
        public string? Treatment { get; set; } = string.Empty;
        
        [ForeignKey("Diagnosis")]
        [Column("diagnosis_id")]
        public int? DiagnosisId { get; set; }

        public virtual MedCardModel MedCard { get; set; } // Навигационное свойство для медкарты
        public virtual DoctorModel Doctor { get; set; }  // Навигационное свойство для врача
        public virtual DiagnosisModel Diagnosis { get; set; } // Навигационное свойство для диагноза
        
        [NotMapped]
        public string DateTimeString => DateTime.ToString("dd.MM.yy hh:mm");
        
    }
}
