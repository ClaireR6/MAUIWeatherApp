using System;
using System.IO;
using System.Collections.ObjectModel;
using static System.Net.WebRequestMethods;
using static WeatherApp.DeserializeOpenMeteo;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;
        const string API_KEY = "d002ad85ff609ef8c338748a8782f7d4";
        int currentTempF;
        const int FORECAST_DAYS = 14;
        const string iconUrl = "https://openweathermap.org/img/wn/";
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "forecast.json");
        WeatherViewModel weatherViewModel = new WeatherViewModel();
        List<ObservableCollection<HourCard>> Days = new List<ObservableCollection<HourCard>>();

        Dictionary<int, WeatherCode> weatherPairs;

        public MainPage()
        {
            InitializeComponent();
            PopulateWeatherPairs();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Call the function to retrieve weather data
            await GetWeatherInfo();
        }


        private async Task PopulateDailyForecast(DeserializeOpenMeteo.WeatherResponse OpenMeteoForecast)
        {
            ObservableCollection<Card> DailyCards = new ObservableCollection<Card> { };
            var daily = OpenMeteoForecast.daily;
            for (int lcv = 0; lcv < FORECAST_DAYS; lcv++)
            {
                string dateTimeString = daily.time[lcv];
                DateTime parsedDate = DateTime.Parse(dateTimeString);
                string formattedDate = parsedDate.ToString("ddd, dd");
                WeatherCode weather = weatherPairs[daily.weather_code[lcv]];

                // Create a new card and populate its properties with the daily forecast data
                Card card = new Card
                {
                    Date = formattedDate,
                    Icon = $"{weather.DayIcon}", // Constructing icon URL
                    High = $"{daily.temperature_2m_max[lcv]}°F", // Max temperature
                    Low = $"{daily.temperature_2m_min[lcv]}°F",  // Min temperature
                    Weather = weather.Description // Weather description
                };

                // Add the new card to the ObservableCollection
                DailyCards.Add(card);
            }

            DailyCardsViewModel dailyCardsViewModel = new DailyCardsViewModel();
            dailyCardsViewModel.DailyCards = DailyCards;
            dailyCardsViewModel.previous = dailyCardsViewModel.DailyCards[0];
            dailyCardsViewModel.previous.IsSelected = true;

            weatherViewModel.DailyCardsViewModel = dailyCardsViewModel;
        }

        private async Task PopulateHourlyForecast(DeserializeOpenMeteo.WeatherResponse OpenMeteoForecast)
        {
            
            var hourly = OpenMeteoForecast.hourly;
            for (int lcv = 0; lcv < FORECAST_DAYS; lcv++)
            {
                ObservableCollection<HourCard> HourlyCards = new ObservableCollection<HourCard> { };
                for (int lcv2 = 0; lcv2 < 24; lcv2++)
                {
                    string dateTimeString = hourly.time[lcv*24 + lcv2];
                    DateTime parsedDate = DateTime.Parse(dateTimeString);
                    string formattedDate = parsedDate.ToString("hh:mm tt");
                    WeatherCode weather = weatherPairs[hourly.weather_code[lcv*24 + lcv2]];

                    // Create a new card and populate its properties with the daily forecast data
                    HourCard card = new HourCard
                    {
                        Time = formattedDate,
                        Icon = $"{weather.DayIcon}", // Constructing icon URL
                        Temperature = (int)hourly.temperature_2m[lcv*24 + lcv2], // Temperature
                        Weather = weather.Description // Weather description
                    };

                    // Add the new card to the ObservableCollection
                    HourlyCards.Add(card);
                }
                Days.Add(HourlyCards);
            }
        }

        private async Task GetWeatherInfo()
        {
            string jsonData;

            // If forecast.json exists w/ prior API info
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                DateTime lastModified = fileInfo.LastWriteTime;

                TimeSpan timeSinceLastModified = DateTime.Now - lastModified;
                if (timeSinceLastModified.Minutes > 15) // Check if 15 minutes have passed since last API call
                {
                    jsonData = await _apiService.GetDataFromOpenMeteoAsync(FORECAST_DAYS);
                    System.IO.File.WriteAllText(filePath, jsonData);
                }
                else
                {
                    jsonData = System.IO.File.ReadAllText(filePath);
                }
            }
            else
            {
                jsonData = await _apiService.GetDataFromOpenMeteoAsync(FORECAST_DAYS);
                System.IO.File.WriteAllText(filePath, jsonData);
            }

            string locationResult = await _apiService.GetLocationNameAsync();


            // OneCall 
            //var oneCallForecast = DeserializeWeather.GetOneCallForecast(result);

            var OpenMeteoForecast = DeserializeOpenMeteo.GetForecast(jsonData);

            await PopulateDailyForecast(OpenMeteoForecast);
            await PopulateHourlyForecast(OpenMeteoForecast);

            HourlyCardsViewModel hourlyCardsViewModel = new HourlyCardsViewModel();
            hourlyCardsViewModel.HourlyCards = Days[0];

            weatherViewModel.HourlyCardsViewModel = hourlyCardsViewModel;

            BindingContext = weatherViewModel;

            var location = DeserializeLocation.GetCurrentLocation(locationResult)[0];

            LocationLbl.Text = location.name;
            currentTempF = (int)OpenMeteoForecast.current.temperature_2m;
            currentTempLbl.Text = $"{currentTempF}°";
            WeatherCode weatherCode = weatherPairs[OpenMeteoForecast.current.weather_code];


            weatherIcon.Source = $"{weatherCode.DayIcon}";


            //DisplayAlert("API Response", $"Temperature: {forecast.main.temp}, Feels Like: {forecast.main.feels_like}", "OK");
        }

        private void ToFahrenheit(object sender, EventArgs e)
        {
            currentTempLbl.Text = $"{currentTempF}°";
            fBtn.Opacity = 1;
            cBtn.Opacity = 0.5;
        }

        private void ToCelsius(object sender, EventArgs e)
        {
            var tempInCelsius = (int)((currentTempF - 32) / 1.8);
            currentTempLbl.Text = $"{tempInCelsius}°";
            fBtn.Opacity = 0.5;
            cBtn.Opacity = 1;
        }

        // On new daily card selected, change background color & hourly info
        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var previous = e.PreviousSelection.FirstOrDefault() as Card;
            var current = e.CurrentSelection.FirstOrDefault() as Card;

            // Update background color of the previously selected card (if it exists)
            if (previous != null)
            {
                previous.IsSelected = false;
            }
            else
            {
                var weatherView = BindingContext as WeatherViewModel;
                var dailyCardsViewModel = weatherView.DailyCardsViewModel;
                if (dailyCardsViewModel?.DailyCards?.Any() == true)
                {
                    dailyCardsViewModel.previous.IsSelected = false;
                }
            }

            // current information
            if (current != null)
            {
                current.IsSelected = true;
                int index = weatherViewModel.DailyCardsViewModel.DailyCards.IndexOf(current);
                weatherViewModel.HourlyCardsViewModel.HourlyCards = Days[index];
                
            }
        }

        private void PopulateWeatherPairs()
        {
            WeatherCode temp = new WeatherCode { Description = "Scattered Clouds", DayIcon = "day2.png", NightIcon = "day2.png" };
            weatherPairs = new Dictionary<int, WeatherCode>()
            {
                {0, new WeatherCode { Description = "Clear Sky", DayIcon = "day0.png", NightIcon = "night0.png" }},
                { 1, new WeatherCode { Description = "Partly Cloudy", DayIcon = "day1.png", NightIcon = "night1.png" }},
                { 2, new WeatherCode { Description = "Scattered Clouds", DayIcon = "day2.png", NightIcon = "day2.png" }},
                { 3, new WeatherCode { Description = "Overcast", DayIcon = "day3.png", NightIcon = "day3.png" }},
                { 45, temp },
                { 48, temp },
                { 65, new WeatherCode { Description = "Heavy Rain", DayIcon = "day65.png", NightIcon = "day65.png" }},
                { 66, new WeatherCode { Description = "Freezing Rain", DayIcon = "day66.png", NightIcon = "day66.png" }},
                { 67, new WeatherCode { Description = "Heavy Freezing Rain", DayIcon = "day67.png", NightIcon = "day67.png" }}
            };
            temp = new WeatherCode { Description = "Drizzle", DayIcon = "day51.png", NightIcon = "night51.png" };
            weatherPairs.Add(51, temp);
            weatherPairs.Add(53, temp);
            weatherPairs.Add(55, temp);
            temp = new WeatherCode { Description = "Freezing Drizzle", DayIcon = "day56.png", NightIcon = "night56.png" };
            weatherPairs.Add(56, temp);
            weatherPairs.Add(57, temp);
            temp = new WeatherCode { Description = "Rain", DayIcon = "day61.png", NightIcon = "night61.png" };
            weatherPairs.Add(61, temp);
            weatherPairs.Add(63, temp);
            temp = new WeatherCode { Description = "Snow", DayIcon = "day70.png", NightIcon = "day70.png" };
            weatherPairs.Add(71, temp);
            weatherPairs.Add(73, temp);
            weatherPairs.Add(75, temp);
            weatherPairs.Add(77, temp);
            temp = new WeatherCode { Description = "Rain Showers", DayIcon = "day80.png", NightIcon = "night80.png" };
            weatherPairs.Add(80, temp);
            weatherPairs.Add(81, temp);
            weatherPairs.Add(82, temp);
            temp = new WeatherCode { Description = "Snow Showers", DayIcon = "day85.png", NightIcon = "night85.png" };
            weatherPairs.Add(85, temp);
            weatherPairs.Add(86, temp);
            temp = new WeatherCode { Description = "Thunderstorm", DayIcon = "day95.png", NightIcon = "night95.png" };
            weatherPairs.Add(95, temp);
            weatherPairs.Add(96, temp);
            weatherPairs.Add(99, temp);

        }
    }

    internal class WeatherCode()
    {
        public string Description { get; set; }
        public string DayIcon { get; set; }
        public string NightIcon { get; set; }

    }

    internal class Hour()
    {
        public string Time { get; set; }
        public int Temperature { get; set; }
        public string Icon { get; set; }
        public string Weather { get; set; }
    }
}