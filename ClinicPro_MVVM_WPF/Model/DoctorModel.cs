
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class DoctorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        
        [Required]
        [StringLength(30)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(30)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(30)]
        [Column("patronymic")]
        public string? Patronymic { get; set; }
        
        [StringLength(5)]
        [Column("office_number")]
        public string? OfficeNumber { get; set; }
        
        [Required]
        [Column("gender")]
        public char Gender { get; set; }
        
        [EmailAddress]
        [Column("email")]
        public string? Email { get; set; }
        
        [Phone]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        
        [Column("photo")]
        public byte[]? Photo { get; set; } // Для хранения изображения
        
        [Required]
        [Column("hash_password")]
        public string HashPassword { get; set; } = string.Empty;
        
        [Required]
        [Column("specializ_id")]
        [ForeignKey("Specialization")]
        public int SpecializId { get; set; }
        
        public virtual SpecializationModel Specialization { get; set; }
    }
}
