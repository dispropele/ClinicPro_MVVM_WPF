using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

namespace ClinicPro_MVVM_WPF.View.Patient.Appointment;

public partial class AppointmentView : UserControl
{
    public AppointmentView(AppointmentParentVM parentVM, int patientId)
    {
        InitializeComponent();
        DataContext = new AppointmentViewVM(parentVM, patientId);
    }
}