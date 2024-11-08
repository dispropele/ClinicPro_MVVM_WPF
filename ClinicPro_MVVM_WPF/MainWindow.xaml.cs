﻿
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

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitMessage exitMessage = new ExitMessage();
            
            exitMessage.Owner = this;

            // Вычисляем положение окна выхода
            double ownerCenterX = this.Left + this.Width / 2;
            double ownerCenterY = this.Top + this.Height / 2;
            exitMessage.Left = ownerCenterX - exitMessage.Width / 2;
            exitMessage.Top = ownerCenterY - exitMessage.Height / 2;
            
            if (exitMessage.ShowDialog() == true)
            {
                this.Close();
            }
        }
    }
}