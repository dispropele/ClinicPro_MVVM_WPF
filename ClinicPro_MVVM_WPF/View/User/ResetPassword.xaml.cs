using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.ViewModel;

namespace ClinicPro_MVVM_WPF.View.User;

public partial class ResetPassword : UserControl
{
    public ResetPassword(AuthVM parentVm, UserModel user)
    {
        InitializeComponent();
        DataContext = new ResetPasswordVM(parentVm, user);
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