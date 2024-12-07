
using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor;


namespace ClinicPro_MVVM_WPF.View.Doctor
{
    /// <summary>
    /// Логика взаимодействия для Appointment.xaml
    /// </summary>
    public partial class Appointment : UserControl
    {
        public Appointment(int doctorId)
        {
            InitializeComponent();
            Console.WriteLine(doctorId);
            DataContext = new AppointmentVM(doctorId);
        }
    }
}
