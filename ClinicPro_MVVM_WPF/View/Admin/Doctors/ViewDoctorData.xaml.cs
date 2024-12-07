using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Admin;

namespace ClinicPro_MVVM_WPF.View.Admin.Doctors;

public partial class ViewDoctorData : UserControl
{
    public ViewDoctorData(DoctorsVM parentVm, int doctorId)
    {
        InitializeComponent();
        DataContext = new ViewDoctorDataVM(parentVm, doctorId);
    }
}