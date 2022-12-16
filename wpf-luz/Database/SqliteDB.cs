using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using wpf_luz.Models;
using wpf_luz.Util;

namespace wpf_luz.Database
{
    public class SqliteDB : IDatabase
    {
        private SQLiteConnection sqliteConnection;
        private SQLiteDataReader reader;
        private readonly string connString = "Data Source=/magic/db.sqlite;";
        private readonly string path = "/magic/db.sqlite";

        public SqliteDB() { }

        private SQLiteConnection GetConnection()
        {
            if (!File.Exists(path))
            {
                CreateDB();
                ResetTables();
                return GetConnection();
            }
            else
            {
                sqliteConnection = new SQLiteConnection(connString);
                sqliteConnection.Open();
                return sqliteConnection;
            }
        }

        public void CreateDB()
        {
            try
            {
                string dir = @"C:\magic";
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                SQLiteConnection.CreateFile(path);
            }
            catch
            {
                throw;
            }
        }

        public ObservableCollection<Deck> GetAllDecks(string sql)
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    ObservableCollection<Deck> list = new ObservableCollection<Deck>();
                    cmd.CommandText = sql;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Deck deck = new Deck()
                        {
                            Id = reader.GetInt32(0),
                            Name = GetStringSafely(reader, 1),
                            Description = GetStringSafely(reader, 2),
                            Cards = GetStringSafely(reader, 3)
                        };
                        list.Add(deck);
                    }
                    return list;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Deck GetDeck(int deckId, string sql)
        {

            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    Deck deck = new Deck();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("id", deckId);
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        deck = new Deck()
                        {
                            Id = reader.GetInt32(0),
                            Name = GetStringSafely(reader, 1),
                            Description = GetStringSafely(reader, 2),
                            Cards = GetStringSafely(reader, 3)
                        };
                    }
                    return deck;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDeck(Deck deck, string sql)
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("name", ReturnEmptyIfNull(deck, "Name"));
                    cmd.Parameters.AddWithValue("description", ReturnEmptyIfNull(deck, "Description"));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveDeck(Deck deck, string sql)
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("id", deck.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDeck(Deck deck, string sql)
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("name", ReturnEmptyIfNull(deck, "Name"));
                    cmd.Parameters.AddWithValue("description", ReturnEmptyIfNull(deck, "Description"));
                    cmd.Parameters.AddWithValue("id", deck.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void AddCardsToDeck(Deck obj, string sql)
        {
            string json = JsonUtil.returnJSON(obj);
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("cards", json);
                    cmd.Parameters.AddWithValue("id", obj.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                throw;
            }
        }

        public void ResetTables()
        {
            using (var cmd = GetConnection().CreateCommand())
            {
                dropTables();
                createTables();
            }
        }

        private void dropTables()
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"  DROP TABLE IF EXISTS decks;
                                          DROP TABLE IF EXISTS cards;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void createTables()
        {
            try
            {
                using (var cmd = GetConnection().CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS decks
                        (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT,
                            description TEXT,
                            cards TEXT 
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ReturnEmptyIfNull<T>(T obj, String PropertyName)
        {
            var prop = typeof(T).GetProperty(PropertyName).GetValue(obj, null);
            if (prop != null)
                return (string)prop;
            return string.Empty;
        }

        private string GetStringSafely(SQLiteDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

    }
}


     
