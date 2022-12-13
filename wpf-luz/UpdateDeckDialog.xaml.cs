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
using System.Windows.Shapes;
using wpf_luz.Models;

namespace wpf_luz
{
    /// <summary>
    /// Interaction logic for UpdateDeckDialog.xaml
    /// </summary>
    public partial class UpdateDeckDialog : Window
    {
        public UpdateDeckDialog(Deck deck)
        {
            InitializeComponent();
            DataContext = deck;
            ShowDialog();
        }

        private void Bnt_Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
