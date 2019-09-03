using StreamlineMVVM;
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

namespace CatPersonSearcher
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        public Catalog()
        {
            InitializeComponent();
        }

        private void pageTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!RegexMatch.NumericMatch((sender as TextBox).Text))
            {
                (sender as TextBox).Text = RegexReplace.MakeNumeric((sender as TextBox).Text);
            }

            // This is janky but it works. I could write an event for this, but there are more important things to do.
            if ((sender as TextBox).IsReadOnly)
            {
                (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
            }
        }
        private void pagePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !RegexMatch.NumericMatch(e.Text);
        }
    }
}
