using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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

        public PostgresDB(string host, string port, string username, string pass)
        {
            connectionString = $"Host={host};Port={port};Username={username};Password={pass};";
        }

        public NpgsqlConnection GetConnection()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    Trace.WriteLine("Connection opened");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return connection;
        }
        
        public ObservableCollection<Deck> GetAllDecks(string sql)
        {
            ObservableCollection<Deck> list = null;
            connection = GetConnection();

            try
            {
                list = new ObservableCollection<Deck>();
                cmd = new NpgsqlCommand(sql, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Deck deck = new Deck()
                    {
                        Id = reader.GetInt32(0),
                        Name = GetStringSafely(reader, 1),
                        Description = GetStringSafely(reader, 2)
                    };
                    list.Add(deck);
                }
                reader.Dispose();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
            return list;
        }

        public Deck GetDeck(int deckId, string sql)
        {
            Deck deck = null;
            connection = GetConnection();

            try
            {
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
                reader.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
            return deck;
        }

        public void InsertDeck(Deck deck, string sql)
        {
            connection = GetConnection();

            try
            {
                cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("name", ReturnEmptyIfNull(deck, "Name"));
                cmd.Parameters.AddWithValue("description", ReturnEmptyIfNull(deck, "Description"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
        }

        public void RemoveDeck(Deck deck, string sql)
        {
            connection = GetConnection();

            try
            {
                cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("id", deck.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
        }

        public void UpdateDeck(Deck deck, string sql)
        {
            connection = GetConnection();

            try
            {
                cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("name", ReturnEmptyIfNull(deck, "Name"));
                cmd.Parameters.AddWithValue("description", ReturnEmptyIfNull(deck, "Description"));
                cmd.Parameters.AddWithValue("id", deck.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
        }

        public void AddCardsToDeck(Deck deck, string sql)
        {
            string json = JsonUtil.returnJSON(deck);
            connection = GetConnection();

            try
            {
                cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("cards", json);
                cmd.Parameters.AddWithValue("id", deck.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
        }



        //public ObservableCollection<Card> LoadData(string sql)
        //{
        //    ObservableCollection<Card> list = new ObservableCollection<Card>();
        //    try
        //    {
        //        connection = GetConnection();
        //        cmd = new NpgsqlCommand("SELECT * FROM cards", connection);
        //        reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {

        //            Card card = new Card()
        //            {
        //                Id = getStringSafely(reader, 0),
        //                Name = getStringSafely(reader, 1),
        //                Set = getStringSafely(reader, 2),
        //                Set_Type = getStringSafely(reader, 3),
        //                Mana_Cost = getStringSafely(reader, 4),
        //                Oracle_Text = getStringSafely(reader, 5),
        //                Type_Line = getStringSafely(reader, 6),
        //                Power = getStringSafely(reader, 7),
        //                Toughness = getStringSafely(reader, 8),
        //                Loyalty = getStringSafely(reader, 9)
        //            };
        //            list.Add(card);
        //        }
        //        return list;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //        reader.Dispose();
        //        connection.Close();
        //    }
        //}

        //public void InsertCard(Card card)
        //{
        //    try
        //    {
        //        connection = GetConnection();
        //        cmd = new NpgsqlCommand("INSERT INTO cards (" + 
        //                                "id, " +
        //                                "name, " +
        //                                "set, " +
        //                                "set_type, " +
        //                                "mana_cost, " +
        //                                "oracle_text, " +
        //                                "type_line, " +
        //                                "power, " +
        //                                "toughness, " +
        //                                "loyalty) " +
        //                                "VALUES(@i, @n, @s, @st, @m, @o, @ty, @p, @to, @l)",
        //                                connection);
        //        cmd.Parameters.AddWithValue("i", returnEmptyIfNull(card, "Id"));
        //        cmd.Parameters.AddWithValue("n", returnEmptyIfNull(card, "Name"));
        //        cmd.Parameters.AddWithValue("s", returnEmptyIfNull(card, "Set"));
        //        cmd.Parameters.AddWithValue("st", returnEmptyIfNull(card, "Set_Type"));
        //        cmd.Parameters.AddWithValue("m", returnEmptyIfNull(card, "Mana_Cost"));
        //        cmd.Parameters.AddWithValue("o", returnEmptyIfNull(card, "Oracle_Text"));
        //        cmd.Parameters.AddWithValue("ty", returnEmptyIfNull(card, "Type_Line"));
        //        cmd.Parameters.AddWithValue("p", returnEmptyIfNull(card, "Power"));
        //        cmd.Parameters.AddWithValue("to", returnEmptyIfNull(card, "Toughness"));
        //        cmd.Parameters.AddWithValue("l", returnEmptyIfNull(card, "Loyalty"));
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //        connection.Close();
        //    }
        //}

        public void ResetTables()
        {
            connection = GetConnection();
            dropTables();
            createTables();
            connection.Close();
        }

        private void dropTables()
        {
            connection = GetConnection();

            try
            {
                cmd = new NpgsqlCommand(@"  DROP TABLE IF EXISTS decks;
                                            DROP TABLE IF EXISTS cards;",
                                            connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
        }

        private void createTables()
        {
            connection = GetConnection();

            try
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

                //CREATE TABLE IF NOT EXISTS cards
                //(
                //    id character varying(50) COLLATE pg_catalog.""default"" NOT NULL,
                //    name character varying(255) COLLATE pg_catalog.""default"",
                //    set character varying(255) COLLATE pg_catalog.""default"",
                //    set_type character varying(255) COLLATE pg_catalog.""default"",
                //    mana_cost character varying(50) COLLATE pg_catalog.""default"",
                //    oracle_text text COLLATE pg_catalog.""default"",
                //    type_line character varying(255) COLLATE pg_catalog.""default"",
                //    power character varying(5) COLLATE pg_catalog.""default"",
                //    toughness character varying(5) COLLATE pg_catalog.""default"",
                //    loyalty character varying(5) COLLATE pg_catalog.""default"",
                //    CONSTRAINT cards_pkey PRIMARY KEY (id)
                //);", connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }

            connection.Close();
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
