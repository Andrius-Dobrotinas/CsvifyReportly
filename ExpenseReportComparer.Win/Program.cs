using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Andy.ExpenseReport.Comparer.Win
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var stateFile = new System.IO.FileInfo("state");
            var defaultSettingsFile = new System.IO.FileInfo(Verifier.Cmd.Program.DefaultSettingsFileName);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(stateFile, defaultSettingsFile));
        }
    }
}
