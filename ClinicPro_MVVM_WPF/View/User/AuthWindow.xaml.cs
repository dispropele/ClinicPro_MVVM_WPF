using ClinicPro_MVVM_WPF.View.Messages;
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

namespace ClinicPro_MVVM_WPF.View.User
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
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
