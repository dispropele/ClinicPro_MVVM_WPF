
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class MedCardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("medcard_id")]
        public int medcardId { get; set; }
        
        [Required]
        [Column("start_date")]
        public DateTime startDate { get; set; }
        
        [Required]
        [ForeignKey("Patient")]
        [Column("patient_id")]
        public int patientId { get; set; } // Внешний ключ для пациента

        public virtual PatientModel Patient { get; set; } // Навигационное свойство для пациента

    }
}
