using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Admin;

namespace ClinicPro_MVVM_WPF.View.Admin.Doctors;

public partial class Doctors : UserControl
{
    public Doctors()
    {
        InitializeComponent();
        DataContext = new DoctorsVM();
    }
}