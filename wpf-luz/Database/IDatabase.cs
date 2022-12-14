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
    public interface IDatabase
    {
        // decks
        ObservableCollection<Deck> GetAllDecks(string sql);
        Deck GetDeck(int deckId, string sql);
        void InsertDeck(Deck obj, string sql);
        void UpdateDeck(Deck obj, string sql);
        void RemoveDeck(Deck obj, string sql);
        void AddCardsToDeck(Deck obj, string sql);
        void ResetTables();


        //cards
        //ObservableCollection<Card> GetAllCards();
        //void InsertCard(Card card);

    }
}
