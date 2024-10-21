using System.ComponentModel.DataAnnotations;


namespace ClinicPro_MVVM_WPF.Model.Patient
{
    public class PatientModel
    {

        public int PatientId { get; set; }
        public string PolisNumber { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public char Gender { get; set; } // "M" или "F"
        [Phone]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string HashPassword { get; private set; }

        public PatientModel()
        {
            PolisNumber = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Patronymic = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }
    }
}
