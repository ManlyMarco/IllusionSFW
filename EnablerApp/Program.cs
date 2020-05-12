using System;
using System.IO;
using System.Windows.Forms;

namespace EnablerApp
{
    static class Program
    {
        internal static string _configPath;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            var dir = Path.GetDirectoryName(typeof(Program).Assembly.Location);

            if (!File.Exists(Path.Combine(dir, "abdata/abdata")))
            {
                MessageBox.Show("The game was not detected in current folder. Make sure you installed this mod directly in the game directory.", "Missing game files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dllPath = Path.Combine(dir, "BepInEx/patchers", "KK_SFW_Patcher.dll");
            if (!File.Exists(dllPath))
            {
                MessageBox.Show("Could not find some of this mod's files. Make sure you installed this mod correctly and try again.", "Missing mod files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var configDir = Path.Combine(dir, "BepInEx/config");

            if (!Directory.Exists(configDir))
            {
                MessageBox.Show("It looks like BepInEx v5.0 or later is not installed. Install the latest compatible version of BepInEx and try again.", "Missing mod files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _configPath = Path.Combine(configDir, "KK_SFW.cfg");


            Application.Run(new SfwSelectWindow());
        }
    }
}
