using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;
using dyscalculia_helper_lib;


namespace dyscalculia_helper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MainWindow _window = new();
        private readonly SettingsManager _settings = SettingsManager.Instance;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up hotkey(s)
            if (_settings.Hotkeys.Count > 0)
            {
                foreach (var hotkey in _settings.Hotkeys)
                {
                    HotkeyManager.Current.AddOrReplace("ShowWindow", hotkey.Key, hotkey.ModifierKey, OnHotkeyPressed);
                }
            }
        }

        private void ShowWindowAndSetPositionToMouse()
        {
            _window.Show();
            Win32Helper.GetCursorPos(out Win32Helper.POINT mousePosition);
            _window.Left = mousePosition.X - (_window.Width / 2);
            _window.Top = mousePosition.Y - (_window.Height - 20);
            _window.Activate();
        }

        private async void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            var selectedText = Win32Helper.GetSelectedText();
            decimal numberSelected = decimal.MinValue;

            if (selectedText != null) 
            {
                // Check if the selected text contains any decimal / thousand separators, and if so, prompt the user to pick one
                if (selectedText.Contains('.') || selectedText.Contains(','))
                {
                    ShowWindowAndSetPositionToMouse();
                    char decimalSeparator = await _window.DetermineDecimalSeparator(selectedText);

                    if (decimalSeparator == ',')
                    {
                        selectedText = selectedText.Replace(".", "");
                    }
                    else if (decimalSeparator == '.')
                    {
                        selectedText = selectedText.Replace(",", "");
                    }

                    numberSelected = ParseNumberToHuman.AttemptParseNumber(selectedText, decimalSeparator);
                }
                else
                {
                    numberSelected = ParseNumberToHuman.AttemptParseNumber(selectedText);
                }
            }

            if (numberSelected != decimal.MinValue)
            {
                var numberFormats = ParseNumberToHuman.ConvertNumberToFormats(numberSelected, _settings.DecimalSeparator);
                Console.WriteLine(numberFormats);
                _window.UpdateNumbersDisplay(numberFormats);

                ShowWindowAndSetPositionToMouse();
            }
        }

    }

}
