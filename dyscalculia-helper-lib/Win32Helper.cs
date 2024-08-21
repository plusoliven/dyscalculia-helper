using System.Runtime.InteropServices;
using System.Windows;

namespace dyscalculia_helper_lib
{
    public class Win32Helper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT cursorPoint);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private const int VK_CONTROL = 0x11;
        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        // https://stackoverflow.com/a/572325
        private static void CopyText()
        {
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(0x43, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(0x43, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }

        public static string GetSelectedText()
        {
            CopyText();
            Thread.Sleep(100);
            return Clipboard.GetText();
        }

        public static string GetSelectedTextAndRevertClipboard()
        {
            throw new NotImplementedException("Broken");
            var currentText = Clipboard.GetText();
            Thread.Sleep(100);
            CopyText();
            Thread.Sleep(100);
            var selectedText = Clipboard.GetText();
            SetClipboardTextSafely(currentText);
            return selectedText;
        }

        // https://stackoverflow.com/a/69081
        // Not a nice solution :(
        private static void SetClipboardTextSafely(string str)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Clipboard.SetText(str);
                    return;
                }
                catch { }
                Thread.Sleep(10);
            }
        }

    }

}
