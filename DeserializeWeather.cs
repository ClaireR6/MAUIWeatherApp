using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmithyApp
{
    internal class DeserializeWeather
    {
        public class OneCallWeatherResponse
        {
            public Current current { get; set; }
            public List<Daily> daily { get; set; }
        }
        
        public class CurrentWeatherResponse
        {
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
        }



        public class Current
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public List<Weather> weather { get; set; }
        }

        public class Daily
        {
            public int dt { get; set; }
            public Temp temp { get; set; }
            public List<Weather> weather { get; set; }
        }

        public class Temp
        {
            public double day { get; set; }
            public double min { get; set; }
            public double max { get; set; }
        }

        public class Weather
        {
            public string? main { get; set; }
            public string? description { get; set; }
            public string? icon { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }

        }
        public static CurrentWeatherResponse GetCurrentForecast(string result)
        {
            CurrentWeatherResponse? WeatherResponse = JsonSerializer.Deserialize<CurrentWeatherResponse>(result);
            return WeatherResponse;
        }

        public static OneCallWeatherResponse GetOneCallForecast(string result)
        {
            OneCallWeatherResponse? OneCallResponse = JsonSerializer.Deserialize<OneCallWeatherResponse>(result);
            return OneCallResponse;
        }
    }
}
