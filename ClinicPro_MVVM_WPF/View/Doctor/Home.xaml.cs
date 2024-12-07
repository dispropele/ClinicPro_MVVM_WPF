using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor;

namespace ClinicPro_MVVM_WPF.View.Doctor
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        
        public Home(int doctorId)
        {
            InitializeComponent();
            DataContext = new HomeVM(doctorId);
        }
    }
}
