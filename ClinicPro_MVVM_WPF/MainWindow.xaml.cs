
using ClinicPro_MVVM_WPF.Utils;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClinicPro_MVVM_WPF.View.Messages;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.View.Doctor.MedCard;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;


namespace ClinicPro_MVVM_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        

        private void HeaderDragMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitMessage exitMessage = new ExitMessage();
            
            exitMessage.Owner = this;

            // Вычисляем положение окна выхода
            double ownerCenterX = Left + Width / 2;
            double ownerCenterY = Top + Height / 2;
            exitMessage.Left = ownerCenterX - exitMessage.Width / 2;
            exitMessage.Top = ownerCenterY - exitMessage.Height / 2;
            
            if (exitMessage.ShowDialog() == true)
            {
                Close();
            }
        }
    }
}