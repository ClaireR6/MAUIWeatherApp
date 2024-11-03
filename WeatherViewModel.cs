using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class WeatherViewModel
    {

        public DailyCardsViewModel DailyCardsViewModel { get; set; }
        public HourlyCardsViewModel HourlyCardsViewModel { get; set; }

        public WeatherViewModel(DailyCardsViewModel daily, HourlyCardsViewModel hourly)
        {
            DailyCardsViewModel = daily;
            HourlyCardsViewModel = hourly;
        }

        public WeatherViewModel()
        {
        }
    }
}
