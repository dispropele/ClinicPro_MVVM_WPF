using System.Windows;
using System.Windows.Input;

namespace ClinicPro_MVVM_WPF.View.Messages
{
    /// <summary>
    /// Логика взаимодействия для ExitMessage.xaml
    /// </summary>
    public partial class ExitMessage : Window
    {
        public ExitMessage()
        {
            InitializeComponent();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ExitMessage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == true)
            {
                DialogResult = false;
            }
        }
        
    }
}
