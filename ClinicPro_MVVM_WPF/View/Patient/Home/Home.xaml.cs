using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Home;

namespace ClinicPro_MVVM_WPF.View.Patient.Home;

public partial class Home : UserControl
{
    public Home(int patientId)
    {
        InitializeComponent();
        DataContext = new HomeVM(patientId);
    }
}