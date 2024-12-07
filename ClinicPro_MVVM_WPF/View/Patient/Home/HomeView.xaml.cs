using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home;


namespace ClinicPro_MVVM_WPF.View.Patient.Home;

public partial class HomeView : UserControl
{
    public HomeView(HomeVM parentVm, int patientId)
    {
        InitializeComponent();
        DataContext = new HomeViewVM(parentVm,patientId);
    }
}