using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class Clipboard_
    {
        public static void SetText(string p_Text)
        {
            Thread STAThread = new Thread(
                delegate ()
                {
                // Use a fully qualified name for Clipboard otherwise it
                // will end up calling itself.
                    System.Windows.Forms.Clipboard.SetText(p_Text);
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }
        public static string GetText()
        {
            string ReturnValue = string.Empty;
            Thread STAThread = new Thread(
                delegate ()
                {
        // Use a fully qualified name for Clipboard otherwise it
        // will end up calling itself.
                    string clipboardText = System.Windows.Forms.Clipboard.GetText();
                    if (!String.IsNullOrEmpty(clipboardText))
                    {
                        ReturnValue = clipboardText;
                    }
                });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();

            return ReturnValue;
        }
    }
}
