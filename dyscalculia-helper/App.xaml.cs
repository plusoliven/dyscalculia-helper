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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            HotkeyManager.Current.AddOrReplace("ShowWindow", Key.D, ModifierKeys.Control, OnHotkeyPressed);
        }

        private void OnHotkeyPressed(object sender, HotkeyEventArgs e)
        {
            // Get selected number
            // If none is selected, or the selected number is the same as the one before, hide the window
            // Else, show the window with the updated number
            var selectedText = Win32Helper.GetSelectedText();
            Console.WriteLine("Selected text - " + selectedText);

            float? numberSelected = null;
            if (float.TryParse(selectedText, out float parsedNumber))
            {
                numberSelected = parsedNumber;
            }

            Console.WriteLine("Parsed text as float - " + numberSelected);

            if (numberSelected != null) 
            {
                Win32Helper.GetCursorPos(out Win32Helper.POINT mousePosition);

                _window.Left = mousePosition.X - (_window.Width / 2);
                _window.Top = mousePosition.Y - (_window.Height - 20);

                _window.Show();
                _window.Activate();
                _window.UpdateNumberDisplay(numberSelected);
            }
    
        }

    }

}
