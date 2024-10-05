using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
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

            if (_settings.Hotkeys.Count > 0)
            {
                foreach (var hotkey in _settings.Hotkeys)
                {
                    HotkeyManager.Current.AddOrReplace("ShowWindow", hotkey.Key, hotkey.ModifierKey, OnHotkeyPressed);
                }
            }
        }

        private async void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            var selectedText = Win32Helper.GetSelectedText();
            decimal numberSelected = decimal.MinValue;

            if (selectedText != null) 
            {
                // Check if the selected text contains non-numeric characters, but still a full number
                var regexMatch = new Regex(@"\d[0-9,\.]*\d").Match(selectedText);
                if (regexMatch.Success) {
                    selectedText = regexMatch.Value;
                }
                else
                {
                    return;
                }

                // Check if the selected text contains any decimal / thousand separators, and if so, prompt the user to pick one
                if (selectedText.Contains('.') || selectedText.Contains(','))
                {
                    _window.ShowWindow();
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
                _window.UpdateNumbersDisplay(numberFormats);

                _window.ShowWindow();
            }
        }

    }

}
