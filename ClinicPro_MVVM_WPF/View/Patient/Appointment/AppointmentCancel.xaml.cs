using System.Windows.Controls;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Appointment;

namespace ClinicPro_MVVM_WPF.View.Patient.Appointment;

public partial class AppointmentCancel : UserControl
{
    public AppointmentCancel(AppointmentParentVM parentVm, AppointmentModel model)
    {
        InitializeComponent();
        DataContext = new AppointmentCancelVM(parentVm, model);
    }
}