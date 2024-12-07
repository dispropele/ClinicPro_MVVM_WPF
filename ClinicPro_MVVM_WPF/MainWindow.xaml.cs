using System.Windows;
using ClinicPro_MVVM_WPF.ViewModel;
using ClinicPro_MVVM_WPF.Data;

namespace ClinicPro_MVVM_WPF
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVm(new ClinicDbContext());
        }
        
    }
}