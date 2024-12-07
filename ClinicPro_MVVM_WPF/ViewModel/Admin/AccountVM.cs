using System.IO;
using System.Windows;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Utils;

namespace ClinicPro_MVVM_WPF.ViewModel.Admin;

public class AccountVM : BaseViewModel
{
    
    public ICommand ExitToLoginCommand { get; set; }
    
    private void ExitAndRestart(object obj)
    {
        var result = MessageBox.Show("Вы уверены, что хотите выйти из аккаунта?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            string filePath = "login_data.json";
            try
            {
                // Удаление файла login_data.json
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                Thread.Sleep(1000);
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                // Обработка ошибки удаления файла (логирование, сообщение пользователю)
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
            }
        }

    }

    public AccountVM()
    {
        ExitToLoginCommand = new RelayCommand(ExitAndRestart);
    } 
    
    
}