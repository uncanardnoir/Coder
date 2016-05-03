using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProblemSolver
{
    class CAutoMutex : IDisposable {
        private Mutex mut;
        bool disposed = false;
        public CAutoMutex(Mutex m)
        {
            mut = m;
            m.WaitOne();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                mut.ReleaseMutex();
            }
            disposed = true;
        }
    }

    class CAutoScopeGuard
    {
        public delegate void CleanupMethod();
        private CleanupMethod cleanupMethod;
        bool disposed = false;
        public CAutoScopeGuard(CleanupMethod cleanup)
        {
            cleanupMethod = cleanup;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                cleanupMethod();
            }
            disposed = true;
        }
    }

    class ThreadingHelper
    {
        public delegate object SelfContainedMethod();
        public delegate void SuccessCallback(long ElapsedMilliseconds, object output);
        public delegate void AbortCallback(long ElapsedMilliseconds);

        private Mutex mutex = new Mutex();
        private Thread ExecutionThread;
        public void RunMethodAsync(SelfContainedMethod method, SuccessCallback successCallback, AbortCallback abortCallback) {
            ThreadPool.QueueUserWorkItem(delegate
            {
                ExecutionThread = Thread.CurrentThread;

                // Register kill thread
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        Thread.Sleep(2000);
                        using (CAutoMutex autoMutex = new CAutoMutex(mutex))
                        {
                            if (null != ExecutionThread)
                            {
                                ExecutionThread.Abort();
                                ExecutionThread.Join();
                            }
                        }
                    });

                    object output = method();
                    sw.Stop();

                    using (CAutoMutex autoMutex = new CAutoMutex(mutex))
                    {
                        successCallback(sw.ElapsedMilliseconds, output);
                        ExecutionThread = null;
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    abortCallback(sw.ElapsedMilliseconds);
                }
            });
        }
    }

    public static class RTBExtensions
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
            if (tsmi.GetCurrentParent().InvokeRequired) {
                tsmi.GetCurrentParent().Invoke(new MethodInvoker(delegate { tsmi.Text += text; }));
                return;
            }
            tsmi.Text += text;
        }

        public static void InvokeEnable(this ToolStripMenuItem tsmi, bool value) {
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
