using System.Windows.Controls;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.ViewModel.Admin;

namespace ClinicPro_MVVM_WPF.View.Admin.Doctors;

public partial class ResetPasswordDoctor : UserControl
{
    public ResetPasswordDoctor(DoctorsVM parentVm, DoctorModel doctor)
    {
        InitializeComponent();
        DataContext = new ResetDoctorVM(parentVm, doctor);
    }
}