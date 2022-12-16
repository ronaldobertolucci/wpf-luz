using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpf_luz.Models;
using wpf_luz.ViewModels;

namespace wpf_luz
{
    /// <summary>
    /// Interaction logic for DeckWindow.xaml
    /// </summary>
    public partial class DeckWindow : Window
    {
        public DeckWindow(ObservableCollection<Card> db, Deck deck)
        {
            InitializeComponent();
            DataContext = new DeckWindowVM(db, deck);
            ShowDialog();
        }

        private void Bnt_Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void deckList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deckList.SelectedItem != null)
                removeBtn.IsEnabled = true;
            else
                removeBtn.IsEnabled = false;
        }

        private void setList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setList.SelectedItem != null)
                addBtn.IsEnabled = true;
            else
                addBtn.IsEnabled = false;
        }
    }
}
