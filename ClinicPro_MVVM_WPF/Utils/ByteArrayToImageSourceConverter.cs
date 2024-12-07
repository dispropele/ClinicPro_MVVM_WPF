using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ClinicPro_MVVM_WPF.Utils;

public class ByteArrayToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] byteArray && byteArray.Length > 0)
        {
            try
            {
                using (var stream = new MemoryStream(byteArray))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception ex)
            {
                // ��������� ������ (��������, �����������)
                Console.WriteLine($"������ ��� �������������� ������ � �����������: {ex.Message}");
            }
        }

        return null; // ���������� null, ���� ������ ����������� ��� ��������� ������
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}