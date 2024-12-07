using System.Windows.Controls;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Account;

namespace ClinicPro_MVVM_WPF.View.Patient.Account;

public partial class AccountResetPassword : UserControl
{
    public AccountResetPassword(AccountParentVM parentVm, PatientModel patient)
    {
        InitializeComponent();
        DataContext = new AccountResetPasswordVM(parentVm, patient);
    }
}