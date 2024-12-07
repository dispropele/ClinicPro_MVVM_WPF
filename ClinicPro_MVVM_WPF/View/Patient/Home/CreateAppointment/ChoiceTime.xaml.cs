using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home.CreateAppointment;

namespace ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment;

public partial class ChoiceTime : UserControl
{
    public ChoiceTime(HomeVM parentVm, int patientId, int doctorId)
    {
        InitializeComponent();
        DataContext = new ChoiceTimeVM(parentVm, patientId, doctorId);
    }
}