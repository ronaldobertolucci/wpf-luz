using Autofac.Extras.Moq;
using Moq;
using System.Collections.ObjectModel;
using wpf_luz.Database;
using wpf_luz.Models;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void GetAllDecks_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string sql = "SELECT id, name, description FROM decks";
                mock.Mock<IDatabase>()
                    .Setup(x => x.GetAllDecks(sql))
                    .Returns(GetSampleDecks());

                var cls = mock.Create<Controller>();
                var expected = GetSampleDecks();
                var actual = cls.GetAllDecks();

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
            }
        }

        [Fact]
        public void GetDeck_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deck = GetSampleDecks()[0];
                string sql = "SELECT * FROM decks WHERE id = @id";
                mock.Mock<IDatabase>()
                    .Setup(x => x.GetDeck(1, sql))
                    .Returns(deck);

                var cls = mock.Create<Controller>();
                var expected = GetSampleDecks()[0];
                var actual = cls.GetDeck(1);

                Assert.True(actual != null);
                Assert.Equal(expected.Name, actual.Name);
            }
        }

        [Fact]
        public void InsertDeck_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deck = GetSampleDecks()[0];
                string sql = "INSERT INTO decks (name, description) " +
                             "VALUES(@name, @description)";

                mock.Mock<IDatabase>()
                    .Setup(x => x.InsertDeck(deck, sql));

                var cls = mock.Create<Controller>();
                cls.InsertDeck(deck);

                // verifica se o método foi chamado somente uma vez
                mock.Mock<IDatabase>()
                    .Verify(x => x.InsertDeck(deck, sql), Times.Exactly(1));
            }
        }

        [Fact]
        public void UpdateDeck_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deck = GetSampleDecks()[0];
                string sql = "UPDATE decks " +
                             "SET name = @name, " +
                             "description = @description " +
                             "WHERE id = @id";

                mock.Mock<IDatabase>()
                    .Setup(x => x.UpdateDeck(deck, sql));

                var cls = mock.Create<Controller>();
                cls.UpdateDeck(deck);

                mock.Mock<IDatabase>()
                    .Verify(x => x.UpdateDeck(deck, sql), Times.Exactly(1));
            }
        }

        [Fact]
        public void RemoveDeck_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deck = GetSampleDecks()[0];
                string sql = "DELETE FROM decks WHERE id = @id";

                mock.Mock<IDatabase>()
                    .Setup(x => x.RemoveDeck(deck, sql));

                var cls = mock.Create<Controller>();
                cls.RemoveDeck(deck);

                mock.Mock<IDatabase>()
                    .Verify(x => x.RemoveDeck(deck, sql), Times.Exactly(1));
            }
        }

        [Fact]
        public void AddCardsToDeck_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var deck = GetSampleDecks()[0];
                string sql = "UPDATE decks " +
                             "SET cards = @cards " +
                             "WHERE id = @id";

                mock.Mock<IDatabase>()
                    .Setup(x => x.AddCardsToDeck(deck, sql));

                var cls = mock.Create<Controller>();
                cls.AddCardsToDeck(deck);

                mock.Mock<IDatabase>()
                    .Verify(x => x.AddCardsToDeck(deck, sql), Times.Exactly(1));
            }
        }

        [Fact]
        public void ResetTables_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDatabase>()
                    .Setup(x => x.ResetTables());

                var cls = mock.Create<Controller>();
                cls.ResetTables();

                mock.Mock<IDatabase>()
                    .Verify(x => x.ResetTables(), Times.Exactly(1));
            }
        }

        private ObservableCollection<Deck> GetSampleDecks()
        {
            ObservableCollection<Deck> output = new ObservableCollection<Deck>()
            {
                new Deck()
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Test",
                    Cards = "Test"
                },
                new Deck()
                {
                    Id = 2,
                    Name = "Test 2",
                    Description = "Test 2",
                    Cards = "Test 2"
                },
                new Deck()
                {
                    Id = 3,
                    Name = "Test 3",
                    Description = "Test 3",
                    Cards = "Test 3"
                },
            };
            return output;
        }
    }
}