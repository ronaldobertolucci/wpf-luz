using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using wpf_luz.Models;
using wpf_luz.Util;

namespace wpf_luz.Database
{
    public class PostgresDB : IDatabase
    {
        private NpgsqlCommand cmd;
        private NpgsqlDataReader reader;
        NpgsqlConnection connection;
        private readonly string connectionString;
        private string host = "127.0.0.1";
        private string port = "5432";
        private string username = "postgres";
        private string pass = "docker";

        public PostgresDB()
        {
            connectionString = $"Host={host};Port={port};Username={username};Password={pass};";
        }

        public NpgsqlConnection GetConnection()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception)
            {
                throw;
            }
            return connection;
        }
        
        public ObservableCollection<Deck> GetAllDecks(string sql)
        {

            try
            {
                using (connection = GetConnection())
                {
                    ObservableCollection<Deck> list = new ObservableCollection<Deck>();
                    cmd = new NpgsqlCommand(sql, connection);
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
            catch (Exception e)
            {
                throw;
            }
        }

        public Deck GetDeck(int deckId, string sql)
        {
            try
            {
                using (connection = GetConnection())
                {
                    Deck deck = new Deck();
                    cmd = new NpgsqlCommand(sql, connection);
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
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(sql, connection);
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
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(sql, connection);
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
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(sql, connection);
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

        public void AddCardsToDeck(Deck deck, string sql)
        {
            string json = JsonUtil.returnJSON(deck);
            connection = GetConnection();

            try
            {
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("cards", json);
                    cmd.Parameters.AddWithValue("id", deck.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ResetTables()
        {
            using (connection = GetConnection())
            {
                dropTables();
                createTables();
            }
        }

        private void dropTables()
        {
            try
            {
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(@"  DROP TABLE IF EXISTS decks;
                                                DROP TABLE IF EXISTS cards;",
                                                connection);
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
                using (connection = GetConnection())
                {
                    cmd = new NpgsqlCommand(@"
                            CREATE TABLE IF NOT EXISTS decks
                            (
                                id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
                                name character varying(50) COLLATE pg_catalog.""default"",
                                description character varying(255) COLLATE pg_catalog.""default"",
                                cards text,
                                CONSTRAINT decks_pkey PRIMARY KEY (id)
                            );", connection);
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

        private string GetStringSafely(NpgsqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}
