
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
        public string Complaints { get; set; }
        
        [Column("examination")]
        public string Examination { get; set; }
        
        [Column("recommendation")]
        public string Recommendation { get; set; }
        
        [Column("treatment")]
        public string Treatment { get; set; }

        public virtual MedCardModel MedCard { get; set; } // Навигационное свойство для медкарты
        public virtual DoctorModel Doctor { get; set; }  // Навигационное свойство для врача
        public virtual DiagnosisModel Diagnosis { get; set; } // Навигационное свойство для диагноза
    }
}
