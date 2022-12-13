using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using wpf_luz.Database;
using wpf_luz.Models;
using wpf_luz.Util;

namespace wpf_luz.ViewModels
{
    public class MainWindowVM
    {
        public ObservableCollection<Card> MagicDB { get; set; }
        public ICommand AddDeck { get; private set; }
        public ICommand RemoveDeck { get; private set; }
        public ICommand EditDeck { get; private set; }
        public ICommand AddCards { get; private set; }
        public ObservableCollection<Deck> Decks { get; set; }
        public Deck selectedDeck { get; set; }
        public IDBController dBController { get; set; }

        public MainWindowVM()
        {
            dBController = new PostgresDB();
            Decks = new ObservableCollection<Deck>();

            MagicDB = JsonUtil.loadJSONCardDatabase(); // load cards
            LoadDecks();

            InicializeCommand();
        }

        public void InicializeCommand()
        {
            AddDeck = new RelayCommand((object _) =>
            {
                Deck deck = new Deck();
                CreateDeckDialog createDeckDialog = new CreateDeckDialog(deck);
                try
                {
                    if (createDeckDialog.DialogResult == true)
                        dBController.InsertDeck(deck);
                }
                catch (Exception)
                {
                    MessageBox.Show("Insert Deck Error");
                }

                LoadDecks();
            });
            RemoveDeck = new RelayCommand((object _) =>
            {
                try
                {
                    dBController.RemoveDeck(selectedDeck);
                }
                catch (Exception)
                {
                    MessageBox.Show("Remove Deck Error");
                }

                LoadDecks();
            });
            EditDeck = new RelayCommand((object _) =>
            {
                UpdateDeckDialog updateDeckDialog = new UpdateDeckDialog(selectedDeck);
                try
                {
                    if (updateDeckDialog.DialogResult == true)
                        dBController.UpdateDeck(selectedDeck);
                }
                catch (Exception)
                {
                    MessageBox.Show("Update Deck Error");
                }

                LoadDecks();
            });
            AddCards = new RelayCommand((object _) =>
            {
                try
                {
                    DeckWindow deckWindow = new DeckWindow(MagicDB, selectedDeck.Id);
                }
                catch (Exception)
                {
                    MessageBox.Show("Cards Management Error");
                }

                LoadDecks();
            });
        }

        private void LoadDecks()
        {
            Decks.Clear();
            ObservableCollection<Deck> list = null;

            try
            {
                list = new ObservableCollection<Deck>(dBController.GetAllDecks());
                for (int i = 0; i < list.Count; i++)
                    Decks.Add(list[i]);
            }
            catch (Exception)
            {
                MessageBox.Show("Error establishing database connection.\nPlease contact our support team");
                Application.Current.Shutdown();
            }
        }

        public bool isListFilled
        {
            get
            {
                if (Decks.Count == 0 || selectedDeck == null)
                    return false;
                else
                    return true;
            }
        }
    }
}
