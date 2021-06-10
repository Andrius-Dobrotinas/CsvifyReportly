using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpenseReportComparer.Win
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        readonly System.IO.FileInfo stateFile;

        public Form1(System.IO.FileInfo stateFile)
        {
            InitializeComponent();
            AllocConsole();

            this.stateFile = stateFile;
        }

        private void button_Select1_Click(object sender, EventArgs e)
        {
            var file = ShowFileSelectDialog(txt_File1.Text);

            if (file != null)
                txt_File1.Text = file;
        }

        private void button_Select2_Click(object sender, EventArgs e)
        {
            var file = ShowFileSelectDialog(txt_File2.Text);

            if (file != null)
                txt_File2.Text = file;
        }

        private void button_SelectOutput_Click(object sender, EventArgs e)
        {
            var file = ShowFileSaveDialog(txt_FileOutput.Text);

            if (file != null)
                txt_FileOutput.Text = file;
        }

        private void button_Go_Click(object sender, EventArgs e)
        {
            Andy.ExpenseReport.Verifier.Cmd.Program.Main(
                new[]
                {
                    $"--source1={txt_File1.Text}",
                    $"--source2={txt_File1.Text}",
                    "--result=c:\\d\\zhaha.txt"
                });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveState();
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
                    Output = txt_FileOutput.Text
                },
                stateFile);
        }

        private void LoadState()
        {
            var state = JsonFileUtil.ReadState(stateFile);

            txt_File1.Text = state.Source1;
            txt_File2.Text = state.Source2;
            //txt_FileOutput.Text = state.Output; // don't load that because of the overwrite prompt
        }
    }
}