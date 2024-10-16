using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace SmithyApp
{
    public class ApiService
    {

        private readonly HttpClient _httpClient;
        const string API_KEY = "b2466fc82c0754b623cfdab853b84bda";
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
    }
}
