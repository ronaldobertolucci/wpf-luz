using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_luz.Models;

namespace wpf_luz.Database
{
    public class Controller
    {
        IDatabase database;
        public Controller() 
        {
            database = new PostgresDB("127.0.0.1", "5432", "postgres", "docker");
        }

        public ObservableCollection<Deck> GetAllDecks()
        {
            string sql = "SELECT id, name, description FROM decks";
            return database.GetAllDecks(sql);
        }

        public Deck GetDeck(int deckId)
        {
            string sql = "SELECT * FROM decks WHERE id = @id";
            return database.GetDeck(deckId, sql);
        }

        public void InsertDeck(Deck deck)
        {
            string sql = "INSERT INTO decks (name, description) " +
                "VALUES(@name, @description)";
            database.InsertDeck(deck, sql);
        }

        public void UpdateDeck(Deck deck)
        {
            string sql = "UPDATE decks " +
                    "SET name = @name, " +
                    "description = @description " +
                    "WHERE id = @id";
            database.UpdateDeck(deck, sql);
        }

        public void RemoveDeck(Deck deck)
        {
            string sql = "DELETE FROM decks WHERE id = @id";
            database.RemoveDeck(deck, sql);
        }

        public void AddCardsToDeck(Deck deck)
        {
            // update focado nos cards
            string sql = "UPDATE decks " +
                    "SET cards = @cards " +
                    "WHERE id = @id";
            database.AddCardsToDeck(deck, sql);
        }

        public void ResetTables()
        { 
            database.ResetTables();
        }
    }
}
