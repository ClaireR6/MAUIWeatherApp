using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace WeatherApp
{
    public class ApiService
    {

        private readonly HttpClient _httpClient;
        const string API_KEY = "d002ad85ff609ef8c338748a8782f7d4";
        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<string>> GetCachedLocationAsync()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                {
                    List<string> latLon = new List<string>() { $"{Math.Round(location.Latitude, 2)}", $"{Math.Round(location.Longitude, 2)}" };
                    return latLon;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., location services unavailable or permission not granted)
                Console.WriteLine($"Error getting location: {ex.Message}");
            }

            return null; // Return null if no location is found or an error occurs
        }

        public async Task<string> GetLocationNameAsync()
        {
            Task<List<string>> coordsTask = GetCachedLocationAsync();
            List<string> coords = await coordsTask;
            string lat = coords[0];
            string lon = coords[1];
            string url = $"http://api.openweathermap.org/geo/1.0/reverse?lat={lat}&lon={lon}&limit=1&appid={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
        // Discontinued call to the OpenWeather API
        public async Task<string> GetDataFromApiAsync()
        {
            Task<List<string>> coordsTask = GetCachedLocationAsync();
            List<string> coords = await coordsTask;
            string lat = coords[0];
            string lon = coords[1];
            string url = $"https://api.openweathermap.org/data/2.5/forecast?units=imperial&lat={lat}&lon={lon}&appid={API_KEY}";
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }

        // An alternate api call using the Open Meteo API
        public async Task<string> GetDataFromOpenMeteoAsync(int forecast_days)
        {
            Task<List<string>> coordsTask = GetCachedLocationAsync();
            List<string> coords = await coordsTask;
            string lat = coords[0];
            string lon = coords[1];
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,weather_code&hourly=temperature_2m,weather_code&daily=temperature_2m_max,temperature_2m_min,weather_code&temperature_unit=fahrenheit&timezone=auto&forecast_days={forecast_days}";
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
    }
}
