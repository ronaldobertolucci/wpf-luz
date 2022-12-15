using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpf_luz.Database;
using wpf_luz.Models;
using wpf_luz.Util;

namespace wpf_luz.ViewModels
{
    public class DeckWindowVM : NotifyPropertyChanged
    {
        public ObservableCollection<Card> MagicDB { get; set; }
        public Deck Deck { get; set; }
        public ICommand AddCard { get; private set; }
        public ICommand RemoveCard { get; private set; }
        public ICommand ClearDeck { get; private set; }
        public ICommand SaveDeck { get; private set; }
        public Card selectedSetCard { get; set; }
        public Card selectedDeckCard { get; set; }
        public Controller dBController { get; set; }

        public DeckWindowVM(ObservableCollection<Card> db, int deckId)
        {
            MagicDB = db;
            dBController = new Controller(new PostgresDB(
                "127.0.0.1", "5432", "postgres", "docker"));
            Initiaze(deckId);
        }

        public void InicializeCommand()
        {
            AddCard = new RelayCommand((object _) =>
            {
                Card card = new Card(selectedSetCard);
                Deck.Add(card);
                Notify(Deck, "Count");
            });
            RemoveCard = new RelayCommand((object _) =>
            {
                Deck.Remove(selectedDeckCard);
                Notify(Deck, "Count");
            });
            ClearDeck = new RelayCommand((object _) =>
            {
                Deck.Clear();
            });
            SaveDeck = new RelayCommand((object _) =>
            {
                try
                {
                    dBController.AddCardsToDeck(Deck);
                }
                catch (Exception)
                {
                    MessageBox.Show("Saving Deck Error");
                }
            });
        }


        public void Initiaze(int deckId)
        {
            try
            {
                Deck = dBController.GetDeck(deckId); // 'cards' null problem
                fillDeck();
                InicializeCommand();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void fillDeck()
        {
            ObservableCollection<Card> cards = JsonUtil.loadCards(Deck.Cards);
            if (cards != null)
            {
                for (int i = 0; i < cards.Count; i++)
                    Deck.Add(cards[i]);
                cards.Clear();
            }
        }

        public bool isDeckSelected
        {
            get
            {
                if (selectedDeckCard != null)
                    return true;
                else
                    return false;
            }
        }

        public bool isSetSelected
        {
            get
            {
                if (selectedSetCard != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
