using System.Net.Http;
using System.Text.Json;
using ClinicPro_MVVM_WPF.Model;

namespace ClinicPro_MVVM_WPF.Service;

public class PatientService
{   
    private const string apiUrl = "http://localhost:8080/api/patient"; //url api для получения данных
    private static HttpClient _httpClient = new HttpClient();

    public async Task<List<PatientModel>> getAllPatients()
    {
        try
        {
            string? responseBody = await _httpClient.GetStringAsync(apiUrl);
            

            List<PatientModel> patients = JsonSerializer.Deserialize<List<PatientModel>>(responseBody);

            if (patients == null && !string.IsNullOrEmpty(responseBody))
            {
                Console.WriteLine("Ошибка десериализации");
                new JsonException();
            }

            return patients;
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
    
    
    public async Task<PatientModel> getPatientById(int id)
    {
        string patientUrl = apiUrl + $"/{id}";

        try
        {
            string? responseBody = await _httpClient.GetStringAsync(patientUrl);

            PatientModel patient = JsonSerializer.Deserialize<PatientModel>(responseBody);

            if (patient == null && !string.IsNullOrEmpty(responseBody))
            {
                Console.WriteLine("Ошибка: Неверный формат JSON");
                new JsonException();
            }

            return patient;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}