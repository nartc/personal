using System;
using System.Windows.Forms;

namespace D3Support
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm f = new MainForm();
            f.Hide();
            Application.Run(f);
        }
    }
}
