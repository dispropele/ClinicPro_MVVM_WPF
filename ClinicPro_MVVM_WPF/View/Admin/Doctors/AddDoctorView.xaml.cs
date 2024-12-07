using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Admin;

namespace ClinicPro_MVVM_WPF.View.Admin.Doctors;

public partial class AddDoctorView : UserControl
{
    public AddDoctorView(DoctorsVM parentVm)
    {
        InitializeComponent();
        DataContext = new AddDoctorVM(parentVm);
    }
}