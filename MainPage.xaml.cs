using System.Collections.ObjectModel;
using static System.Net.WebRequestMethods;

namespace SmithyApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;
        const string API_KEY = "d002ad85ff609ef8c338748a8782f7d4";
        int currentTempF;
        const string iconUrl = "https://openweathermap.org/img/wn/";

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Call the function to retrieve weather data
            await GetWeatherInfo();
        }

        private async Task PopulateDailyForecast(DeserializeWeather.OneCallWeatherResponse oneCallForecast)
        {
            ObservableCollection<Card> DailyCards = new ObservableCollection<Card> { };
            //{
            //    new Card { Date = "Mon, 15 Oct", Icon = "i01d.png", High = "75°F", Low = "55°F", Weather = "Sunny"},
            //    new Card { Date = "Tue, 16 Oct", Icon = "103d.png", High = "78°F", Low = "58°F", Weather = "Cloudy" },
            //    new Card { Date = "Wed, 17 Oct", Icon = "i09d.png", High = "72°F", Low = "50°F", Weather = "Rain" }
            //};
            foreach (var daily in oneCallForecast.daily) 
            {
                // Create a new card and populate its properties with the daily forecast data
                Card card = new Card
                {
                    Date = DateTimeOffset.FromUnixTimeSeconds(daily.dt).ToString("dddd, dd"), // Formatting date
                    Icon = $"https://openweathermap.org/img/wn/{daily.weather[0].icon}.png", // Constructing icon URL
                    High = $"{daily.temp.max}°F", // Max temperature
                    Low = $"{daily.temp.min}°F",  // Min temperature
                    Weather = daily.weather[0].description // Weather description
                };

                // Add the new card to the ObservableCollection
                DailyCards.Add(card);
            }

            DailyCardsViewModel dailyCardsViewModel = new DailyCardsViewModel();
            dailyCardsViewModel.DailyCards = DailyCards;

            BindingContext = dailyCardsViewModel;
        }

        private async Task GetWeatherInfo() 
        {
            string result = await _apiService.GetDataFromApiAsync();
            string locationResult = await _apiService.GetLocationNameAsync();


            var oneCallForecast = DeserializeWeather.GetOneCallForecast(result);
            await PopulateDailyForecast(oneCallForecast);

            var location = DeserializeLocation.GetCurrentLocation(locationResult)[0];

            LocationLbl.Text = location.name;
            currentTempF = (int)oneCallForecast.current.temp;
            currentTempLbl.Text = $"{currentTempF}°";

            //weatherIcon.Source = $"{iconUrl}{forecast.weather[0].icon}.";

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
            var tempInCelsius = (int)((currentTempF-32)/1.8);
            currentTempLbl.Text = $"{tempInCelsius}°";
            fBtn.Opacity = 0.5;
            cBtn.Opacity = 1;
        }

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
                var dailyCardsViewModel = BindingContext as DailyCardsViewModel;
                if (dailyCardsViewModel?.DailyCards?.Any() == true)
                {
                    dailyCardsViewModel.previous.IsSelected = false;
                }
            }

            // Update background color of the currently selected card (if it exists)
            if (current != null)
            {
                current.IsSelected = true;   
            }
        }
    }    
}
