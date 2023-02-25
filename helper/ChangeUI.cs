using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class ChangeUI//เปลี่ยน ui ขณะทำการรันใน thread
    {
        public static void labelname(System.Windows.Forms.Label lb, string msg)
        {
            lb.Invoke(new Action(() =>
            {
                lb.Text = msg;

            }));

        }
        public static void invokedatagridview(System.Windows.Forms.DataGridView DataGridView, object[] msg)
        {
            DataGridView.BeginInvoke(new Action(() =>
            {
                DataGridView.Rows.Add(msg);
                DataGridView.FirstDisplayedScrollingRowIndex = DataGridView.RowCount - 1;
            }));
        }

        public static void CopyImageToClipboard(Image image)
        {
            if (image == null) return;

            Clipboard.SetImage(image);
        }

        public static void CopyImageToClipboardInThread(Image image)
        {
            Thread thread = new Thread(() => CopyImageToClipboard(image));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

    }
}
