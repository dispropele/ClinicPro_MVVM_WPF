using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ClinicPro_MVVM_WPF.Data;
using ClinicPro_MVVM_WPF.Data.Doctor;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using Microsoft.Win32;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor
{
    public partial class AccountVM : BaseViewModel, IDataErrorInfo
    {
        
        public ICommand ExitToLoginCommand  { get; set; }
        public ICommand SelectedPhotoCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand ChangesCommand { get; set; }

        private DoctorRepository _repDoctor;
        
        private int DoctorId { get; set; }
        private DoctorModel _doctor { get; set; }
        
        private byte[] _photo { get; set; }
        public byte[] Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                OnPropertyChanged();
            }
        }
        
        private string _selectedImagePath;
        public string SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }
        
        private async Task LoadDoctor()
        {
            var doctor = await _repDoctor.GetDoctorByIdAsync(DoctorId);
            if (doctor != null)
            {
                _doctor = doctor;
            }
            
        }

        private async Task LoadAccountData()
        {
            await LoadDoctor();
            Photo = _doctor.Photo ?? GetDefaultDoctorImageBytes();
            Email = _doctor.Email ?? string.Empty;
            PhoneNumber = _doctor.PhoneNumber ?? string.Empty;
        }

        private async Task SaveChanges(object obj)
        {
            try
            {
                if (Errors != null && Errors.Any())
                {
                    MessageBox.Show("Пожалуйста, исправьте ошибки в форме", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                var result = MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
                
                _doctor.Email = Email;
                _doctor.PhoneNumber = PhoneNumber;
                
                await _repDoctor.UpdateDoctorAsync(_doctor);
                MessageBox.Show("Успешное обновление данных");

            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка обновления врача: "+e.Message);
            }
        }
        
        private void SelectImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePath = openFileDialog.FileName;
                // Загрузка данных изображения в массив байтов
                try
                {
                    Photo = File.ReadAllBytes(openFileDialog.FileName);
                    _doctor.Photo = Photo; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                }
            }
        }

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
        
        private byte[] GetDefaultDoctorImageBytes()
        {
            try
            {
                // Создаем Uri для иконки, предполагая, что она встроена как Resource
                var uri = new Uri("pack://application:,,,/Resources/doctor.png"); 

                // Загружаем изображение из Uri
                var bitmap = new BitmapImage(uri);

                // Кодируем изображение в PNG и получаем массив байтов
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки (например, логирование)
                Console.WriteLine($"Ошибка при загрузке базовой иконки врача: {ex.Message}");
                return null; 
            }
        }

        private void Changes(object obj)
        {
            ReadOnly = !ReadOnly;
        }
        

        private bool _readOnly = true;
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                OnPropertyChanged();
            }
        }
        
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                _fieldModified[nameof(Email)] = true;
                OnPropertyChanged();
            }
        }
        
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                _fieldModified[nameof(PhoneNumber)] = true;
                OnPropertyChanged();
            }
        }
        
        private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string PhoneRegex = @"^\+?[0-9]{10,15}$";
        
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        private Dictionary<string, bool> _fieldModified = new Dictionary<string, bool>();
        
        [GeneratedRegex(PhoneRegex)]
        private static partial Regex MyPhoneRegex();
        [GeneratedRegex(EmailRegex)]
        private static partial Regex MyEmailRegex();
        
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                
                // Добавляем проверку на модификацию поля
                if (!_fieldModified.ContainsKey(columnName) || !_fieldModified[columnName])
                    return string.Empty;
                
                switch (columnName)
                {
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "Введите почту";
                        else if (!MyEmailRegex().IsMatch(Email))
                            error = "Некорректная почта";
                        break;
                    case nameof(PhoneNumber):
                        if (string.IsNullOrWhiteSpace(PhoneNumber))
                            error = "Введите номер";
                        else if (!MyPhoneRegex().IsMatch(PhoneNumber))
                            error = "Некорректный номер";
                        break;
                    
 
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Errors[columnName] = error;
                }
                else
                {
                    Errors.Remove(columnName);
                }
            
                OnPropertyChanged(nameof(Errors));
                return error;
            }
        }
        
        public string Error => null;
        
        private bool IsCorrect(object param)
        {
            return !Errors.Any() || Errors == null;
        }
        
        public AccountVM(int doctorId)
        {
            DoctorId = doctorId;
            
            ExitToLoginCommand = new RelayCommand(ExitAndRestart);
            SelectedPhotoCommand = new RelayCommand(SelectImage);
            SaveChangesCommand = new RelayCommand(async o => await SaveChanges(o), IsCorrect);
            ChangesCommand = new RelayCommand(Changes);
            
            var context = new ClinicDbContext();
            _repDoctor = new DoctorRepository(context);

            Task.Run(async () => await LoadAccountData());


        }
    }
}
