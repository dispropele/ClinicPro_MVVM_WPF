using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor;

namespace ClinicPro_MVVM_WPF.View.Doctor
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        public Account(int doctorId)
        {
            InitializeComponent();
            DataContext = new AccountVM(doctorId);
        }
    }
}
