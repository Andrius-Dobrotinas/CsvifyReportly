
namespace ExpenseReportComparer.Win
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_Go = new System.Windows.Forms.Button();
            this.groupBox_Source1 = new System.Windows.Forms.GroupBox();
            this.button_Select1 = new System.Windows.Forms.Button();
            this.txt_File1 = new System.Windows.Forms.TextBox();
            this.groupBox_Source2 = new System.Windows.Forms.GroupBox();
            this.button_Select2 = new System.Windows.Forms.Button();
            this.txt_File2 = new System.Windows.Forms.TextBox();
            this.groupBox_Output = new System.Windows.Forms.GroupBox();
            this.button_SelectOutput = new System.Windows.Forms.Button();
            this.txt_FileOutput = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox_Settings = new System.Windows.Forms.GroupBox();
            this.button_OpenSettingsFile = new System.Windows.Forms.Button();
            this.button_SelectSettings = new System.Windows.Forms.Button();
            this.txt_FileSettings = new System.Windows.Forms.TextBox();
            this.groupBox_Source1.SuspendLayout();
            this.groupBox_Source2.SuspendLayout();
            this.groupBox_Output.SuspendLayout();
            this.groupBox_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_Go
            // 
            this.button_Go.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Go.Location = new System.Drawing.Point(7, 272);
            this.button_Go.Name = "button_Go";
            this.button_Go.Size = new System.Drawing.Size(438, 28);
            this.button_Go.TabIndex = 5;
            this.button_Go.Text = "Go";
            this.button_Go.UseVisualStyleBackColor = true;
            this.button_Go.Click += new System.EventHandler(this.button_Go_Click);
            // 
            // groupBox_Source1
            // 
            this.groupBox_Source1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Source1.Controls.Add(this.button_Select1);
            this.groupBox_Source1.Controls.Add(this.txt_File1);
            this.groupBox_Source1.Location = new System.Drawing.Point(7, 102);
            this.groupBox_Source1.Name = "groupBox_Source1";
            this.groupBox_Source1.Size = new System.Drawing.Size(437, 52);
            this.groupBox_Source1.TabIndex = 2;
            this.groupBox_Source1.TabStop = false;
            this.groupBox_Source1.Text = "Source 1";
            // 
            // button_Select1
            // 
            this.button_Select1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Select1.Location = new System.Drawing.Point(355, 20);
            this.button_Select1.Name = "button_Select1";
            this.button_Select1.Size = new System.Drawing.Size(75, 23);
            this.button_Select1.TabIndex = 2;
            this.button_Select1.Text = "Select...";
            this.button_Select1.UseVisualStyleBackColor = true;
            this.button_Select1.Click += new System.EventHandler(this.button_Select1_Click);
            // 
            // txt_File1
            // 
            this.txt_File1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_File1.Location = new System.Drawing.Point(6, 21);
            this.txt_File1.Name = "txt_File1";
            this.txt_File1.Size = new System.Drawing.Size(344, 23);
            this.txt_File1.TabIndex = 1;
            // 
            // groupBox_Source2
            // 
            this.groupBox_Source2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Source2.Controls.Add(this.button_Select2);
            this.groupBox_Source2.Controls.Add(this.txt_File2);
            this.groupBox_Source2.Location = new System.Drawing.Point(7, 158);
            this.groupBox_Source2.Name = "groupBox_Source2";
            this.groupBox_Source2.Size = new System.Drawing.Size(437, 52);
            this.groupBox_Source2.TabIndex = 3;
            this.groupBox_Source2.TabStop = false;
            this.groupBox_Source2.Text = "Source 2";
            // 
            // button_Select2
            // 
            this.button_Select2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Select2.Location = new System.Drawing.Point(356, 20);
            this.button_Select2.Name = "button_Select2";
            this.button_Select2.Size = new System.Drawing.Size(75, 23);
            this.button_Select2.TabIndex = 2;
            this.button_Select2.Text = "Select...";
            this.button_Select2.UseVisualStyleBackColor = true;
            this.button_Select2.Click += new System.EventHandler(this.button_Select2_Click);
            // 
            // txt_File2
            // 
            this.txt_File2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_File2.Location = new System.Drawing.Point(6, 21);
            this.txt_File2.Name = "txt_File2";
            this.txt_File2.Size = new System.Drawing.Size(344, 23);
            this.txt_File2.TabIndex = 1;
            // 
            // groupBox_Output
            // 
            this.groupBox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Output.Controls.Add(this.button_SelectOutput);
            this.groupBox_Output.Controls.Add(this.txt_FileOutput);
            this.groupBox_Output.Location = new System.Drawing.Point(7, 214);
            this.groupBox_Output.Name = "groupBox_Output";
            this.groupBox_Output.Size = new System.Drawing.Size(437, 52);
            this.groupBox_Output.TabIndex = 4;
            this.groupBox_Output.TabStop = false;
            this.groupBox_Output.Text = "Output";
            // 
            // button_SelectOutput
            // 
            this.button_SelectOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectOutput.Location = new System.Drawing.Point(355, 20);
            this.button_SelectOutput.Name = "button_SelectOutput";
            this.button_SelectOutput.Size = new System.Drawing.Size(75, 23);
            this.button_SelectOutput.TabIndex = 2;
            this.button_SelectOutput.Text = "Select...";
            this.button_SelectOutput.UseVisualStyleBackColor = true;
            this.button_SelectOutput.Click += new System.EventHandler(this.button_SelectOutput_Click);
            // 
            // txt_FileOutput
            // 
            this.txt_FileOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FileOutput.Location = new System.Drawing.Point(6, 21);
            this.txt_FileOutput.Name = "txt_FileOutput";
            this.txt_FileOutput.ReadOnly = true;
            this.txt_FileOutput.Size = new System.Drawing.Size(344, 23);
            this.txt_FileOutput.TabIndex = 1;
            // 
            // groupBox_Settings
            // 
            this.groupBox_Settings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Settings.Controls.Add(this.button_OpenSettingsFile);
            this.groupBox_Settings.Controls.Add(this.button_SelectSettings);
            this.groupBox_Settings.Controls.Add(this.txt_FileSettings);
            this.groupBox_Settings.Location = new System.Drawing.Point(7, 12);
            this.groupBox_Settings.Name = "groupBox_Settings";
            this.groupBox_Settings.Size = new System.Drawing.Size(437, 84);
            this.groupBox_Settings.TabIndex = 1;
            this.groupBox_Settings.TabStop = false;
            this.groupBox_Settings.Text = "Settings File";
            // 
            // button_OpenSettingsFile
            // 
            this.button_OpenSettingsFile.Location = new System.Drawing.Point(6, 50);
            this.button_OpenSettingsFile.Name = "button_OpenSettingsFile";
            this.button_OpenSettingsFile.Size = new System.Drawing.Size(167, 23);
            this.button_OpenSettingsFile.TabIndex = 3;
            this.button_OpenSettingsFile.Text = "Open The File";
            this.button_OpenSettingsFile.UseVisualStyleBackColor = true;
            this.button_OpenSettingsFile.Click += new System.EventHandler(this.button_OpenSettingsFile_Click);
            // 
            // button_SelectSettings
            // 
            this.button_SelectSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectSettings.Location = new System.Drawing.Point(356, 20);
            this.button_SelectSettings.Name = "button_SelectSettings";
            this.button_SelectSettings.Size = new System.Drawing.Size(75, 23);
            this.button_SelectSettings.TabIndex = 2;
            this.button_SelectSettings.Text = "Select...";
            this.button_SelectSettings.UseVisualStyleBackColor = true;
            this.button_SelectSettings.Click += new System.EventHandler(this.button_SelectSettings_Click);
            // 
            // txt_FileSettings
            // 
            this.txt_FileSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FileSettings.Location = new System.Drawing.Point(6, 21);
            this.txt_FileSettings.Name = "txt_FileSettings";
            this.txt_FileSettings.ReadOnly = true;
            this.txt_FileSettings.Size = new System.Drawing.Size(344, 23);
            this.txt_FileSettings.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 309);
            this.Controls.Add(this.groupBox_Settings);
            this.Controls.Add(this.groupBox_Output);
            this.Controls.Add(this.groupBox_Source2);
            this.Controls.Add(this.groupBox_Source1);
            this.Controls.Add(this.button_Go);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_Source1.ResumeLayout(false);
            this.groupBox_Source1.PerformLayout();
            this.groupBox_Source2.ResumeLayout(false);
            this.groupBox_Source2.PerformLayout();
            this.groupBox_Output.ResumeLayout(false);
            this.groupBox_Output.PerformLayout();
            this.groupBox_Settings.ResumeLayout(false);
            this.groupBox_Settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_Go;
        private System.Windows.Forms.GroupBox groupBox_Source1;
        private System.Windows.Forms.Button button_Select1;
        private System.Windows.Forms.TextBox txt_File1;
        private System.Windows.Forms.GroupBox groupBox_Source2;
        private System.Windows.Forms.Button button_Select2;
        private System.Windows.Forms.TextBox txt_File2;
        private System.Windows.Forms.GroupBox groupBox_Output;
        private System.Windows.Forms.Button button_SelectOutput;
        private System.Windows.Forms.TextBox txt_FileOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox_Settings;
        private System.Windows.Forms.Button button_SelectSettings;
        private System.Windows.Forms.TextBox txt_FileSettings;
        private System.Windows.Forms.Button button_OpenSettingsFile;
    }
}

