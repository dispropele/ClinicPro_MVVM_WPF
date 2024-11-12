
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class AppointmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointment_id")]
        public int AppointmentId { get; set; }
        
        [Required]
        [ForeignKey("Patient")]
        [Column("patient_id")]
        public int PatientId { get; set; }
        
        [Required]
        [ForeignKey("Doctor")]
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        
        [Required]
        [Column("date_time")]
        public DateTime DateTime { get; set; }
        
        [Required]
        [Column("status")]
        public string Status { get; set; } // "Запланирован", "Выполнен", "Отменен"
        
        [Column("reason_cancel")]
        public string? ReasonCancel { get; set; }


        public virtual PatientModel Patient { get; set; } = null!; // Навигационное свойство для пациента
        public virtual DoctorModel Doctor { get; set; } = null!; // Навигационное свойство для врача

    }
}
