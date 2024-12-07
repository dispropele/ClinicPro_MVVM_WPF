using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

namespace ClinicPro_MVVM_WPF.View.Patient.Appointment;

public partial class AppointmentParent : UserControl
{
    public AppointmentParent(int patientId)
    {
        InitializeComponent();
        DataContext = new AppointmentParentVM(patientId);
    }
}