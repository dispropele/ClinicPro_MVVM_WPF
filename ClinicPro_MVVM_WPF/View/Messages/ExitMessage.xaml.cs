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
            this.DialogResult = true;
        }
        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ExitMessage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == true)
            {
                this.DialogResult = false;
            }
        }
        
    }
}
