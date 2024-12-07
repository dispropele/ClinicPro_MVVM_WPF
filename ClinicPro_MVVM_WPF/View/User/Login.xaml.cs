using ClinicPro_MVVM_WPF.View.Messages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClinicPro_MVVM_WPF.View.User
{
    
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
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
        
        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Login")
            {
                LoginBox.Text = string.Empty;
                LoginBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                LoginBox.Opacity = 1; // Полная непрозрачность при вводе
            }
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text))
            {
                LoginBox.Text = "Login";
                LoginBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                LoginBox.Opacity = 0.5;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "Password")
            {
                PasswordBox.Clear();
                PasswordBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                PasswordBox.FontStyle = FontStyles.Normal;
                LoginBox.Opacity = 1; // Полная непрозрачность при вводе
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordBox.Password = "Password";
                PasswordBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                PasswordBox.FontStyle = FontStyles.Italic;
                LoginBox.Opacity = 0.5;
            }
        }
    }
}
