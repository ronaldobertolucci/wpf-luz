using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wpf_luz.Models;

namespace wpf_luz.Database
{
    public interface IDBController
    {
        // decks
        ObservableCollection<Deck> GetAllDecks();
        Deck GetDeck(int deckId);
        void InsertDeck(Deck deck);
        void UpdateDeck(Deck deck);
        void RemoveDeck(Deck deck);
        void AddCardsToDeck(Deck deck);


        //cards
        //ObservableCollection<Card> GetAllCards();
        //void InsertCard(Card card);

        void ResetTables();
    }
}
