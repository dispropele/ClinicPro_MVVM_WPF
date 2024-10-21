using ClinicPro_MVVM_WPF.Model.Doctor;
using System.ComponentModel.DataAnnotations;


namespace ClinicPro_MVVM_WPF.Model.Specialization
{
    public class SpecializationModel
    {
        public int SpecializationId { get; set; }

        [Required]
        public string Name { get; set; } // Название специализации

        public virtual ICollection<DoctorModel> Doctors { get; set; } = new HashSet<DoctorModel>();
    }
}
