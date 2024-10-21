
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
                this.DragMove();
        }

        private void Exit(object sender, MouseButtonEventArgs e)
        {
            ExitMessage exitMessage = new ExitMessage();
            exitMessage.Show();
            if (exitMessage.isConfirmed)
            {
                this.Close();
            }
        }
    }
}