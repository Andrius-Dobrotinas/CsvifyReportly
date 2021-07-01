
namespace Andy.ExpenseReport.Comparer.Win
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
            this.groupBox_Result = new System.Windows.Forms.GroupBox();
            this.button_SelectResult = new System.Windows.Forms.Button();
            this.txt_FileResult = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox_Settings = new System.Windows.Forms.GroupBox();
            this.button_OpenSettingsFile = new System.Windows.Forms.Button();
            this.button_SelectSettings = new System.Windows.Forms.Button();
            this.txt_FileSettings = new System.Windows.Forms.TextBox();
            this.textbox_Output = new System.Windows.Forms.TextBox();
            this.groupBox_Source1.SuspendLayout();
            this.groupBox_Source2.SuspendLayout();
            this.groupBox_Result.SuspendLayout();
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
            this.button_Go.Size = new System.Drawing.Size(437, 28);
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
            this.txt_File1.AllowDrop = true;
            this.txt_File1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_File1.Location = new System.Drawing.Point(6, 21);
            this.txt_File1.Name = "txt_File1";
            this.txt_File1.Size = new System.Drawing.Size(344, 23);
            this.txt_File1.TabIndex = 1;
            this.txt_File1.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_File1_DragDrop);
            this.txt_File1.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
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
            this.txt_File2.AllowDrop = true;
            this.txt_File2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_File2.Location = new System.Drawing.Point(6, 21);
            this.txt_File2.Name = "txt_File2";
            this.txt_File2.Size = new System.Drawing.Size(344, 23);
            this.txt_File2.TabIndex = 1;
            this.txt_File2.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_File2_DragDrop);
            this.txt_File2.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // groupBox_Result
            // 
            this.groupBox_Result.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Result.Controls.Add(this.button_SelectResult);
            this.groupBox_Result.Controls.Add(this.txt_FileResult);
            this.groupBox_Result.Location = new System.Drawing.Point(7, 214);
            this.groupBox_Result.Name = "groupBox_Result";
            this.groupBox_Result.Size = new System.Drawing.Size(437, 52);
            this.groupBox_Result.TabIndex = 4;
            this.groupBox_Result.TabStop = false;
            this.groupBox_Result.Text = "Result";
            // 
            // button_SelectResult
            // 
            this.button_SelectResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectResult.Location = new System.Drawing.Point(355, 20);
            this.button_SelectResult.Name = "button_SelectResult";
            this.button_SelectResult.Size = new System.Drawing.Size(75, 23);
            this.button_SelectResult.TabIndex = 2;
            this.button_SelectResult.Text = "Select...";
            this.button_SelectResult.UseVisualStyleBackColor = true;
            this.button_SelectResult.Click += new System.EventHandler(this.button_SelectResult_Click);
            // 
            // txt_FileResult
            // 
            this.txt_FileResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FileResult.Location = new System.Drawing.Point(6, 21);
            this.txt_FileResult.Name = "txt_FileResult";
            this.txt_FileResult.ReadOnly = true;
            this.txt_FileResult.Size = new System.Drawing.Size(344, 23);
            this.txt_FileResult.TabIndex = 1;
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
            this.txt_FileSettings.AllowDrop = true;
            this.txt_FileSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FileSettings.Location = new System.Drawing.Point(6, 21);
            this.txt_FileSettings.Name = "txt_FileSettings";
            this.txt_FileSettings.ReadOnly = true;
            this.txt_FileSettings.Size = new System.Drawing.Size(344, 23);
            this.txt_FileSettings.TabIndex = 1;
            this.txt_FileSettings.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_FileSettings_DragDrop);
            this.txt_FileSettings.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // textbox_Output
            // 
            this.textbox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_Output.BackColor = System.Drawing.Color.Black;
            this.textbox_Output.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textbox_Output.ForeColor = System.Drawing.Color.Lime;
            this.textbox_Output.Location = new System.Drawing.Point(7, 315);
            this.textbox_Output.Multiline = true;
            this.textbox_Output.Name = "textbox_Output";
            this.textbox_Output.ReadOnly = true;
            this.textbox_Output.Size = new System.Drawing.Size(437, 98);
            this.textbox_Output.TabIndex = 6;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 425);
            this.Controls.Add(this.textbox_Output);
            this.Controls.Add(this.groupBox_Settings);
            this.Controls.Add(this.groupBox_Result);
            this.Controls.Add(this.groupBox_Source2);
            this.Controls.Add(this.groupBox_Source1);
            this.Controls.Add(this.button_Go);
            this.Name = "Form1";
            this.Text = "Report Verifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_Source1.ResumeLayout(false);
            this.groupBox_Source1.PerformLayout();
            this.groupBox_Source2.ResumeLayout(false);
            this.groupBox_Source2.PerformLayout();
            this.groupBox_Result.ResumeLayout(false);
            this.groupBox_Result.PerformLayout();
            this.groupBox_Settings.ResumeLayout(false);
            this.groupBox_Settings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.GroupBox groupBox_Result;
        private System.Windows.Forms.Button button_SelectResult;
        private System.Windows.Forms.TextBox txt_FileResult;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox_Settings;
        private System.Windows.Forms.Button button_SelectSettings;
        private System.Windows.Forms.TextBox txt_FileSettings;
        private System.Windows.Forms.Button button_OpenSettingsFile;
        private System.Windows.Forms.TextBox textbox_Output;
    }
}

