using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.View.Messages;
using ClinicPro_MVVM_WPF.ViewModel;

namespace ClinicPro_MVVM_WPF.View;

public partial class AdminView : UserControl
{
    public AdminView()
    {
        InitializeComponent();
        DataContext = new AdminNavigationVM();
    }
    private void HeaderDragMove(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            Window.GetWindow(this)?.DragMove();
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        ExitMessage exitMessage = new ExitMessage();
            
        // Ќаходим родительское окно:
        Window parentWindow = Window.GetWindow(this);

        if (parentWindow != null)
        {
            exitMessage.Owner = parentWindow;

            // ¬ычисл€ем положение окна выхода относительно родительского окна
            double ownerCenterX = parentWindow.Left + parentWindow.Width / 2;
            double ownerCenterY = parentWindow.Top + parentWindow.Height / 2;
            exitMessage.Left = ownerCenterX - exitMessage.Width / 2;
            exitMessage.Top = ownerCenterY - exitMessage.Height / 2;

            if (exitMessage.ShowDialog() == true)
            {
                parentWindow.Close(); // «акрываем родительское окно
            }
        }
    }
}