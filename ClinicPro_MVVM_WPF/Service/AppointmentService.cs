using System.Net.Http;
using System.Text.Json;
using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Service;

public class AppointmentService
{
    
    /// <summary>
    /// today
    /// upcoming
    /// </summary>
    
    private const string apiUrl = "http://localhost:8080/api/appointment";
    private static HttpClient _httpClient = new HttpClient();
    
    public async Task<List<AppointmentModel>> getAllAppointments()
    {
        try
        {
            string? responseBody = await _httpClient.GetStringAsync(apiUrl);

            List<AppointmentModel> appointments = JsonSerializer.Deserialize<List<AppointmentModel>>(responseBody);

            if (appointments == null && !string.IsNullOrEmpty(responseBody))
            {
                Console.WriteLine("Ошибка десериализации");
                new JsonException();
            }

            return appointments;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<List<AppointmentModel>> getTodayAppointment(int doctorId)
    {
        try
            {
                string? responseBody = await _httpClient.GetStringAsync($"{apiUrl}/today/{doctorId}");
        
                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    return new List<AppointmentModel>();
                }
        
                List<AppointmentModel> appointments = JsonSerializer.Deserialize<List<AppointmentModel>>(responseBody);
        
                if (appointments == null)
                {
                    throw new JsonException("Ошибка десериализации");
                }
        
                return appointments;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP ошибка: {e.Message}");
                return null;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Ошибка JSON: {e.Message}");
                return null;
            }
    }
}