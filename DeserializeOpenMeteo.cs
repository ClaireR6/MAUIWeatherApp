using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static WeatherApp.DeserializeWeather;

namespace WeatherApp
{
    internal class DeserializeOpenMeteo
    {
        public class WeatherResponse
        {
            public Current current { get; set; }
            public Daily daily { get; set; }
            public Hourly hourly { get; set; }
        }
        public class Current
        {
            public double temperature_2m { get; set; }
            public int weather_code { get; set; }
        }
        
        public class Hourly
        {
            public List<string> time { get; set; }
            public List<double> temperature_2m { get; set; }
            public List<int> weather_code { get; set; }
        }

        public class Daily
        {
            public List<string> time { get; set; }
            public List<double> temperature_2m_max { get; set; }
            public List<double> temperature_2m_min { get; set; }
            public List<int> weather_code { get; set; }
        }

        public static WeatherResponse GetForecast(string result)
        {
            WeatherResponse? WeatherResponse = JsonSerializer.Deserialize<WeatherResponse>(result);
            return WeatherResponse;
        }
    }
}
