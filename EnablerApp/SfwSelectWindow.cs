using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EnablerApp
{
    public partial class SfwSelectWindow : Form
    {
        public SfwSelectWindow()
        {
            InitializeComponent();

            if (File.Exists(Program._configPath))
            {
                var configLine = File.ReadAllLines(Program._configPath)
                    .FirstOrDefault(x => x.StartsWith("Disable NSFW content ="));

                radioButton1.Checked = true;

                if (configLine != null)
                {
                    var enableNsfw = configLine.ToLowerInvariant().Contains("false");
                    radioButtonNsfw.Checked = enableNsfw;
                    radioButtonSfw.Checked = !enableNsfw;
                }
            }
            buttonAcc.Enabled = radioButtonNsfw.Checked || radioButtonSfw.Checked;
        }

        private void radioButtonSfw_CheckedChanged(object sender, EventArgs e)
        {
            buttonAcc.Enabled = radioButtonNsfw.Checked || radioButtonSfw.Checked;
        }

        private void radioButtonNsfw_CheckedChanged(object sender, EventArgs e)
        {
            buttonAcc.Enabled = radioButtonNsfw.Checked || radioButtonSfw.Checked;
        }

        private void buttonAcc_Click(object sender, EventArgs e)
        {
            var nsfw = radioButtonNsfw.Checked;
            try
            {
                File.WriteAllText(Program._configPath, "[General]\r\n\r\nDisable NSFW content = " + (!nsfw).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to write plugin configuration - " + ex, "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Close();
        }

        private void buttonCanc_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
