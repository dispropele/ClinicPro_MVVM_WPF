using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home.CreateAppointment;

namespace ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment;

public partial class ChoiceDoctors : UserControl
{
    public ChoiceDoctors(HomeVM parentVm, int patientId)
    {
        InitializeComponent();
        DataContext = new ChoiceDoctorsVM(parentVm, patientId);
    }
}