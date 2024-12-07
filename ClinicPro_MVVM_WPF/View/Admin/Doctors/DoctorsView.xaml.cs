using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Admin;

namespace ClinicPro_MVVM_WPF.View.Admin;

public partial class DoctorsView : UserControl
{
    public DoctorsView(DoctorsVM parentVm)
    {
        InitializeComponent();
        DataContext = new DoctorsListVM(parentVm);
    }
}