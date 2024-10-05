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
        private TaskCompletionSource<char> decimalSeparatorTcs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Hide();
        }

        public async Task<char> DetermineDecimalSeparator(string selectedText)
        {
            ShowNumbersGrid.Visibility = Visibility.Collapsed;
            SelectDecimalGrid.Visibility = Visibility.Visible;

            SelectDecimalCharSelectedText.Text = selectedText;

            decimalSeparatorTcs = new TaskCompletionSource<char>();
            char result = await decimalSeparatorTcs.Task;

            SelectDecimalGrid.Visibility = Visibility.Collapsed;

            return result;
        }

        private void CommaButton_Click(object sender, RoutedEventArgs e)
        {
            decimalSeparatorTcs?.SetResult(',');
        }

        private void PeriodButton_Click(object sender, RoutedEventArgs e)
        {
            decimalSeparatorTcs?.SetResult('.');
        }

        public void UpdateNumbersDisplay(ParseNumberToHuman.NUMBERFORMATS formats)
        {
            SelectDecimalGrid.Visibility = Visibility.Collapsed;
            ShowNumbersGrid.Visibility = Visibility.Visible;

            NumberDisplay.Text = formats.Number;
            ThousandsSeparatedDisplay.Text = formats.ThousandsSeparated;
            NumberWordsDisplay.Text = formats.Words;
            // PhoneNumberDisplay.Text = formats.PhoneNumber;
        }
    }
}