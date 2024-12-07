using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Account;

namespace ClinicPro_MVVM_WPF.View.Patient.Account;

public partial class AccountView : UserControl
{
    public AccountView(AccountParentVM parentVm, int patientId)
    {
        InitializeComponent();
        DataContext = new AccountViewVM(parentVm, patientId);
    }
}