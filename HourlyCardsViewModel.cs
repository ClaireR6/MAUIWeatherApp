using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class HourlyCardsViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<HourCard> _hourlyCards;

        public ObservableCollection<HourCard> HourlyCards
        {
            get => _hourlyCards;
            set
            {
                if (_hourlyCards != value)
                {
                    _hourlyCards = value;
                    OnPropertyChanged(nameof(HourlyCards));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HourlyCardsViewModel()
        {
            _hourlyCards = new ObservableCollection<HourCard>();
        }


        public void addCard(HourCard card)
        {
            HourlyCards.Add(card);
        }
    }

    public class HourCard
    {
        public string Time { get; set; }
        public string Icon { get; set; }
        public int Temperature { get; set; }
        public string Weather { get; set; }
    }
}
