using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProblemSolver
{
    static class ExtensionMethods
    {
        public static void AppendText(this RichTextBox box, string text, Color color, bool bold = false)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(new MethodInvoker(delegate { box.AppendText(text, color, bold); }));
                return;
            }
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.SelectionFont = new Font(box.Font, FontStyle.Bold);
            box.AppendText(text);
            box.SelectionFont = new Font(box.Font, FontStyle.Regular);
            box.SelectionColor = box.ForeColor;
        }

        public static void InvokeAppendText(this RichTextBox box, string input)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(new MethodInvoker(delegate { box.AppendText(input); }));
                return;
            }
            box.AppendText(input);
        }

        public static string ReadNoncommentLine(this StreamReader sr)
        {
            string s;
            do
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    return null;
                }
            } while (s.TrimStart().StartsWith("#"));
            return s;
        }

        public static void InvokeAppendText(this ToolStripMenuItem tsmi, string text)
        {
            if (tsmi.GetCurrentParent().InvokeRequired)
            {
                tsmi.GetCurrentParent().Invoke(new MethodInvoker(delegate { tsmi.Text += text; }));
                return;
            }
            tsmi.Text += text;
        }

        public static void InvokeEnable(this ToolStripMenuItem tsmi, bool value)
        {
            if (tsmi.GetCurrentParent().InvokeRequired)
            {
                tsmi.GetCurrentParent().Invoke(new MethodInvoker(delegate { tsmi.Enabled = value; }));
                return;
            }
            tsmi.Enabled = value;
        }

        public static bool ApproximatelyEqual(this double d1, double d2, double epsilon = 0.000001)
        {
            return Math.Abs(d1 - d2) < epsilon;
        }
    }
}
