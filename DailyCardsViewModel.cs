using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithyApp
{
    internal class DailyCardsViewModel
    {
        public ObservableCollection<Card> DailyCards { get; set; }
        public Card previous;
        public DailyCardsViewModel()
        {
            DailyCards = new ObservableCollection<Card>
            {
                new Card { Date = "Mon, 15 Oct", Icon = "i01d.png", High = "75°F", Low = "55°F", Weather = "Sunny"},
                new Card { Date = "Tue, 16 Oct", Icon = "103d.png", High = "78°F", Low = "58°F", Weather = "Cloudy" },
                new Card { Date = "Wed, 17 Oct", Icon = "i09d.png", High = "72°F", Low = "50°F", Weather = "Rain" }
            };

            if (DailyCards.Any())
            {
                DailyCards[0].IsSelected = true;
                previous = DailyCards[0];
            }
        }
    }

    public class Card: INotifyPropertyChanged
    {
        public string Date { get; set; }
        public string Icon { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Weather { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        public Color BackgroundColor => IsSelected ? Color.FromArgb("#40FFFFFF") : Colors.Transparent;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
