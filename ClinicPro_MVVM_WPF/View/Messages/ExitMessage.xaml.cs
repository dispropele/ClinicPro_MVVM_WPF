using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClinicPro_MVVM_WPF.View.Messages
{
    /// <summary>
    /// Логика взаимодействия для ExitMessage.xaml
    /// </summary>
    public partial class ExitMessage : Window
    {
        public bool isConfirmed { get; private set; } = false;

        public ExitMessage()
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

    }
}
