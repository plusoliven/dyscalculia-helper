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
        private string _lastSelectedText = "";

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

        public void FetchSelectedText()
        {
            var selectedText = Win32Helper.GetSelectedText();

            if (selectedText == null)
                return;

            // Check if the selected text contains non-numeric characters, but still a full number
            var regexMatch = new Regex(@"\d[0-9,\.]*\d").Match(selectedText);

            if (regexMatch.Success)
                selectedText = regexMatch.Value;
            else
                return;

            if (selectedText.Length > Decimal.MaxValue.ToString().Length)
                return;

            _lastSelectedText = selectedText;
        }

        public void UpdateMainWindow()
        {
            if (string.IsNullOrEmpty(_lastSelectedText))
                return;

            decimal numberSelected;

            // Check if the selected text contains any decimal / thousand separators, and if so, prompt the user to pick one
            if (_lastSelectedText.Contains('.') || _lastSelectedText.Contains(','))
            {
                _window.ShowWindow();
                char decimalSeparator = _window.DecimalSeparator;

                _lastSelectedText = decimalSeparator == ','
                    ? _lastSelectedText.Replace(".", "")
                    : _lastSelectedText.Replace(",", "");

                numberSelected = ParseNumberToHuman.AttemptParseNumber(_lastSelectedText, decimalSeparator);
            }
            else
            {
                numberSelected = ParseNumberToHuman.AttemptParseNumber(_lastSelectedText);
            }

            if (numberSelected == decimal.MinValue)
                return;

            var numberFormats = ParseNumberToHuman.ConvertNumberToFormats(numberSelected, _settings.DecimalSeparator);
            _window.UpdateNumbersDisplay(numberFormats);

            _window.ShowWindow();
        }

        private void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            FetchSelectedText();
            UpdateMainWindow();
        }
    }
}
