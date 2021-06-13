using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExpenseReportComparer.Win
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        readonly System.IO.FileInfo stateFile;
        readonly System.IO.FileInfo defaultSettingsFile;

        public Form1(System.IO.FileInfo stateFile, System.IO.FileInfo defaultSettingsFile)
        {
            InitializeComponent();
            AllocConsole();

            this.stateFile = stateFile;
            this.defaultSettingsFile = defaultSettingsFile;
        }

        private void button_Select1_Click(object sender, EventArgs e)
        {
            var file = ShowFileSelectDialog(txt_File1.Text);

            if (file != null)
                txt_File1.Text = file;

            RefreshReadiness();
        }

        private void button_Select2_Click(object sender, EventArgs e)
        {
            var file = ShowFileSelectDialog(txt_File2.Text);

            if (file != null)
                txt_File2.Text = file;

            RefreshReadiness();
        }

        private void button_SelectResult_Click(object sender, EventArgs e)
        {
            var file = ShowFileSaveDialog(txt_FileResult.Text);

            if (file != null)
                txt_FileResult.Text = file;

            RefreshReadiness();
        }

        private void button_SelectSettings_Click(object sender, EventArgs e)
        {
            var file = ShowFileSelectDialog(txt_FileSettings.Text);

            if (file != null)
                txt_FileSettings.Text = file;

            RefreshReadiness();
        }

        private void button_Go_Click(object sender, EventArgs e)
        {
            Andy.ExpenseReport.Verifier.Cmd.Program.Main(
                new[]
                {
                    $"{Andy.ExpenseReport.Verifier.Cmd.Parameter.Keys.Source1}={txt_File1.Text}",
                    $"{Andy.ExpenseReport.Verifier.Cmd.Parameter.Keys.Source2}={txt_File1.Text}",
                    $"{Andy.ExpenseReport.Verifier.Cmd.Parameter.Keys.ReportFile}={txt_FileResult.Text}",
                    $"{Andy.ExpenseReport.Verifier.Cmd.Parameter.Keys.SettingsFile}={txt_FileSettings.Text}"
                });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (stateFile.Exists)
                LoadState(stateFile);
            else
                this.txt_FileSettings.Text = defaultSettingsFile.FullName;

            RefreshReadiness();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveState();
        }

        private void button_OpenSettingsFile_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txt_FileSettings.Text))
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = txt_FileSettings.Text,
                        UseShellExecute = true
                    });
            else
                MessageBox.Show("The file does not exist");
        }

        private string ShowFileSelectDialog(string file)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                var f = new System.IO.FileInfo(file);
                if (f.Directory.Exists)
                    openFileDialog1.InitialDirectory = f.Directory.FullName;

                openFileDialog1.FileName = f.Name;
            }
            else
            {
                openFileDialog1.InitialDirectory = "";
                openFileDialog1.FileName = "";
            }

            var result = openFileDialog1.ShowDialog();

            if (result != DialogResult.Cancel)
                return openFileDialog1.FileName;
            return null;
        }

        private string ShowFileSaveDialog(string file)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                var f = new System.IO.FileInfo(file);
                if (f.Directory.Exists)
                    saveFileDialog1.InitialDirectory = f.Directory.FullName;

                saveFileDialog1.FileName = f.Name;
            }
            else
            {
                saveFileDialog1.InitialDirectory = "";
                saveFileDialog1.FileName = "";
            }

            var result = saveFileDialog1.ShowDialog();

            if (result != DialogResult.Cancel)
                return saveFileDialog1.FileName;
            return null;
        }

        private void SaveState()
        {
            JsonFileUtil.SaveState(
                new State
                {
                    Source1 = txt_File1.Text,
                    Source2 = txt_File2.Text,
                    Result = txt_FileResult.Text,
                    SettingsFile = txt_FileSettings.Text
                },
                stateFile);
        }

        private void LoadState(System.IO.FileInfo file)
        {
            var state = JsonFileUtil.ReadState(file);

            txt_File1.Text = state.Source1;
            txt_File2.Text = state.Source2;
            //txt_FileResult.Text = state.Result; // don't load that because of the overwrite prompt

            txt_FileSettings.Text = state.SettingsFile;
            txt_FileSettings.Text = defaultSettingsFile.FullName;
        }

        private void RefreshReadiness()
        {
            bool isReady = !string.IsNullOrWhiteSpace(txt_FileSettings.Text) &&
                !string.IsNullOrWhiteSpace(txt_File1.Text) &&
                !string.IsNullOrWhiteSpace(txt_File2.Text) &&
                !string.IsNullOrWhiteSpace(txt_FileResult.Text);

            button_Go.Enabled = isReady;
        }
    }
}