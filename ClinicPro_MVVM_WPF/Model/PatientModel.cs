
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    
    public class PatientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("patient_id")]
        public int PatientId { get; set; }
        
        [Required]
        [StringLength(50)]
        [Column("polis_number")]
        public string PolisNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(50)]
        [Column("patronymic")]
        public string? Patronymic { get; set; }
        
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }
        
        [Required]
        [StringLength(1)]
        [Column("gender")]
        public char Gender { get; set; } // "M" или "F"
        
        [Phone]
        [StringLength(15)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        [Column("email")]
        public string? Email { get; set; }
        
        [Required]
        [Column("hash_password")]
        public string HashPassword { get; set; }  = string.Empty;
        
        
    }
}
