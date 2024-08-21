using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dyscalculia_helper_lib;

namespace dyscalculia_helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Hide();
        }

        public void UpdateNumbersDisplay(ParseNumberToHuman.NUMBERFORMATS formats)
        {
            NumberDisplay.Text = formats.Number;
            ThousandsSeparatedDisplay.Text = formats.ThousandsSeparated;
            NumberWordsDisplay.Text = formats.Words;
            // PhoneNumberDisplay.Text = formats.PhoneNumber;
        }
    }
}