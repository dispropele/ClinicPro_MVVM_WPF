using System.ComponentModel.DataAnnotations;


namespace ClinicPro_MVVM_WPF.Model.Doctor
{
    public class DoctorModel
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Patronymic { get; set; }
        public string? OfficeNumber { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public byte[]? Photo { get; set; } // Для хранения изображения

        public string HashPassword { get; private set; }
    }
}
