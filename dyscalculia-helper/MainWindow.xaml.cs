using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public char DecimalSeparator { get; private set; }

        private TaskCompletionSource<char> _decimalSeparatorTcs;
        private readonly DoubleAnimation _fadeInAnimation = new DoubleAnimation
        {
            From = 0.0,
            To = 1.0,
            Duration = new Duration(TimeSpan.FromSeconds(0.1))
        };

        public MainWindow()
        {
            InitializeComponent();
            DecimalSeparator = ',';
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

            _decimalSeparatorTcs = new TaskCompletionSource<char>();
            char result = await _decimalSeparatorTcs.Task;

            SelectDecimalGrid.Visibility = Visibility.Collapsed;

            return result;
        }

        public void ShowWindow()
        {
            Win32Helper.GetCursorPos(out Win32Helper.POINT mousePosition);
            this.Left = mousePosition.X - (this.Width / 2);
            this.Top = mousePosition.Y - (this.Height - 20);
            FadeInWindow();
            this.Activate();
            this.Show();
        }

        private void FadeInWindow()
        {
            var storyboard = new Storyboard();
            storyboard.Children.Add(_fadeInAnimation);
            Storyboard.SetTarget(_fadeInAnimation, this);
            Storyboard.SetTargetProperty(_fadeInAnimation, new PropertyPath(OpacityProperty));

            this.BeginStoryboard(storyboard);
        }

        private void CommaButton_Click(object sender, RoutedEventArgs e)
        {
            _decimalSeparatorTcs?.SetResult(',');
        }

        private void PeriodButton_Click(object sender, RoutedEventArgs e)
        {
            _decimalSeparatorTcs?.SetResult('.');
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

        private void RadioDecimalSeparatorComma_Checked(object sender, RoutedEventArgs e)
        {
            DecimalSeparator = ',';
            RadioDecimalSeparatorComma.IsChecked = true;
            RadioDecimalSeparatorPeriod.IsChecked = false;
            var app = (App)Application.Current;
            app.UpdateMainWindow();
        }

        private void RadioDecimalSeparatorPeriod_Checked(object sender, RoutedEventArgs e)
        {
            DecimalSeparator = '.';
            RadioDecimalSeparatorComma.IsChecked = false;
            RadioDecimalSeparatorPeriod.IsChecked = true;
            var app = (App)Application.Current;
            app.UpdateMainWindow();
        }
    }
}