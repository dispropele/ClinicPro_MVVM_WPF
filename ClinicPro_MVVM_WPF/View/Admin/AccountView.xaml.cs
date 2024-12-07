using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor;
using AccountVM = ClinicPro_MVVM_WPF.ViewModel.Admin.AccountVM;

namespace ClinicPro_MVVM_WPF.View.Admin;

public partial class AccountView : UserControl
{
    public AccountView()
    {
        InitializeComponent();
        DataContext = new AccountVM();
    }
}