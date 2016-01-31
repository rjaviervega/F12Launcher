using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace F12Laucher
{
    static class Program
    {
        public static MainForm PrgForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PrgForm = new MainForm();
            Application.Run(PrgForm);
        }
    }
}
