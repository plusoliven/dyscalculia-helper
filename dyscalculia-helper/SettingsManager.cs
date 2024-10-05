using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Input;

namespace dyscalculia_helper_lib
{
    public class SettingsManager
    {
        public struct HOTKEY
        {
            public Key Key;
            public ModifierKeys ModifierKey;
        }

        // Localization
        public NumberFormatInfo NumberFormatInfo { get; private set; }
        public string Language { get; private set; }
        public char DecimalSeparator { get; private set; }

        // Hotkey
        public List<HOTKEY> Hotkeys { get; private set; }

        // Formats to display
        public bool ShowOriginalNumber { get; private set; }
        public bool ShowThousandsSeparated { get; private set; }
        public bool ShowNumberWords { get; private set; }
        public bool ShowPhoneNumber { get; private set; }

        private static SettingsManager? _instance;

        public static SettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsManager();
                }
                return _instance;
            }
        }

        private SettingsManager()
        {
            NumberFormatInfo = new NumberFormatInfo
            {
                NumberGroupSeparator = ","
            };
            Language = "en-US";
            DecimalSeparator = ',';
            Hotkeys =
            [
                new HOTKEY
                {
                    Key = Key.D,
                    ModifierKey = ModifierKeys.Control
                }
            ];
            ShowOriginalNumber = true;
            ShowThousandsSeparated = true;
            ShowNumberWords = true;
            ShowPhoneNumber = true;
        }
    }
}
