using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_luz.ViewModels;

namespace wpf_luz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
        }

        private void deckList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deckList.SelectedItem != null)
            {
                editBtn.IsEnabled = true;
                removeBtn.IsEnabled = true;
                cardsBtn.IsEnabled = true;
            }
            else
            {
                editBtn.IsEnabled = false;
                removeBtn.IsEnabled = false;
                cardsBtn.IsEnabled = false;
            }
        }
    }
}
