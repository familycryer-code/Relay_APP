using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace RelayControl
{
    static class Program
    {
        private static Mutex m_Mutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            m_Mutex = new Mutex(true, "DIGITALGRID, INC. Relay UI", out createdNew);
#if !DEBUG
            if (createdNew)
                Application.Run(new MainControl());
            else
            {
                MessageBox.Show("The application is already running.", Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
#else
            Application.Run(new MainControl());
#endif
        }
    }
}