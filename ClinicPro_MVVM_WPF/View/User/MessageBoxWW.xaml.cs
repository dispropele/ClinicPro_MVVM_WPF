using System.Windows;
using System.Windows.Input;

namespace ClinicPro_MVVM_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxWW.xaml
    /// </summary>
    public partial class MessageBoxWW : Window
    {
        public bool isConfirmed { get; private set; } = false;

        public MessageBoxWW()
        {
            InitializeComponent();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            isConfirmed = true;
            this.Close();
        }
        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            isConfirmed = false;
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
