namespace WeatherApp
{
	public partial class DailyCard : ContentView
	{

		public static readonly BindableProperty DateProperty = BindableProperty.Create(
			nameof(Date),
			typeof(string),
			typeof(DailyCard),
			default(string));

		public string Date
		{
			get => (string)GetValue(DateProperty); 
			set => SetValue(DateProperty, value);
		}

        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(DailyCard),
            default(string));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty HighProperty = BindableProperty.Create(
            nameof(High),
            typeof(string),
            typeof(DailyCard),
            default(string));

        public string High
        {
            get => (string)GetValue(HighProperty);
            set => SetValue(HighProperty, value);
        }

        public static readonly BindableProperty LowProperty = BindableProperty.Create(
            nameof(Low),
            typeof(string),
            typeof(DailyCard),
            default(string));

        public string Low
        {
            get => (string)GetValue(LowProperty);
            set => SetValue(LowProperty, value);
        }

        public static readonly BindableProperty WeatherProperty = BindableProperty.Create(
            nameof(Weather),
            typeof(string),
            typeof(DailyCard),
            default(string));

        public string Weather
        {
            get => (string)GetValue(WeatherProperty);
            set => SetValue(WeatherProperty, value);
        }

        public DailyCard() {
        	InitializeComponent();
        }
    }
}