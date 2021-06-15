using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Andy.ExpenseReport.Comparer.Win
{
    /// <summary>
    /// Writes the output to the supplied <see cref="TextBoxBase"/>.
    /// Most methods are not overridden, I didn't need them.
    /// </summary>
    public class TextBoxWriter : TextWriter
    {
        private readonly TextBoxBase textBox;

        public override Encoding Encoding => Encoding.UTF8;

        public TextBoxWriter(TextBoxBase textBox)
        {
            this.textBox = textBox;
        }

        public override void WriteLine(string value)
        {
            if (textBox.TextLength == 0)
                textBox.AppendText(value);
            else
                textBox.AppendText(Environment.NewLine + value);
        }

        public override void WriteLine(string format, object arg0)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string format, params object[] arg)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            throw new NotImplementedException();
        }
    }
}