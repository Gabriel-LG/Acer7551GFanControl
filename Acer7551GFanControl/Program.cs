using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acer7551GFanControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Regulator regulator = null;
            try
            {
                regulator = new Regulator();
                TrayIcon context = new TrayIcon(regulator);
                Application.Run(context);
                regulator.Stop();
            }
            catch (Exception e)
            {
                if (regulator != null) regulator.Stop();
                MessageBox.Show(e.ToString(), "Acer7551G Fan Control", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
