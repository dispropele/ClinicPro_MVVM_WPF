
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class SpecializationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("specializ_id")]
        public int SpecializId { get; set; }
        
        [Required]
        [Column("name_specialization")]
        public string NameSpecialization { get; set; } // Название специализации
        
    }
}
