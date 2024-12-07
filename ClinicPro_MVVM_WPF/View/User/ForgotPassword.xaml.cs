using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.ViewModel;

namespace ClinicPro_MVVM_WPF.View.User;

public partial class ForgotPassword : UserControl
{
    public ForgotPassword(AuthVM parentVm)
    {
        InitializeComponent();
        DataContext = new ForgotPasswordVM(parentVm);
    }
    private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            // Получаем родительское окно и вызываем DragMove
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.DragMove();
        }
    }
}