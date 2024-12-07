using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient;
using ClinicPro_MVVM_WPF.ViewModel.Patient.Account;

namespace ClinicPro_MVVM_WPF.View.Patient.Account;

public partial class AccountParent : UserControl
{
    public AccountParent(int patientId)
    {
        InitializeComponent();
        DataContext = new AccountParentVM(patientId);
    }
}