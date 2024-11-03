using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static WeatherApp.DeserializeWeather;

namespace WeatherApp
{
    internal class DeserializeLocation
    {
        public class LocationResponse() 
        {
            public string name { get; set; }
        }

        public static List<LocationResponse> GetCurrentLocation(string result)
        {
            List<LocationResponse>? LocationResponse = JsonSerializer.Deserialize<List<LocationResponse>>(result);
            return LocationResponse;
        }
    }
}
