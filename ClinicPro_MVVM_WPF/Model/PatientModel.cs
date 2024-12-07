
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    
    public class PatientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patient_id")]
        public int patientId { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column("polis_number")]
        public string polisNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string firstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string lastName { get; set; } = string.Empty;
        
        [StringLength(50)]
        [Column("patronymic")]
        public string? patronymic { get; set; } = string.Empty;
        
        [Column("date_of_birth")]
        public DateTime? dateOfBirth { get; set; }
        
        [Required]
        [StringLength(1)]
        [Column("gender")]
        public char gender { get; set; } // "M" или "F"
        
        [Phone]
        [StringLength(15)]
        [Column("phone_number")]
        public string? phoneNumber { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        [Column("email")]
        public string? email { get; set; }
        
        [Column("user_id")]
        [ForeignKey("User")]
        public int userId { get; set; }
        
        public virtual UserModel User { get; set; }
        
        [NotMapped]
        public string dateOfBirthString => $"{dateOfBirth:dd.MM.yyyy}";
        [NotMapped]
        public string Fio => $"{lastName} {firstName} {patronymic}";
        [NotMapped]
        public string Summary => $"{lastName} {firstName[0]}. {patronymic?[0]}. ({dateOfBirth:yyyy}г.)";
    }
}
