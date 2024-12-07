
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicPro_MVVM_WPF.Model
{
    public class DiagnosisModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("diagnosis_id")]
        public int DiagnosisId { get; set; }
        
        [Required]
        [Column("name")]
        public string Name { get; set; }
        
    }
}
