using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class DailyCardsViewModel
    {
        public ObservableCollection<Card> DailyCards { get; set; }
        public Card previous;
        public DailyCardsViewModel()
        {
            DailyCards = new ObservableCollection<Card>();
        }

        public void addCard(Card card)
        {
            DailyCards.Add(card);
            if (DailyCards.Count == 1)
            {
                previous = card;
                card.IsSelected = true;
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
