﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProblemSolver
{
    public partial class MainForm : Form
    {
        string programRoot;
        string problemsRoot;
        CodingProblem currentProblem;
        System.Media.SoundPlayer tada = new System.Media.SoundPlayer(@"C:\windows\media\tada.wav");
        HashSet<int> SolvedProblems = new HashSet<int>();
        bool waitingForResults = false;

        // singleton objects for test
        MethodInfo currentTestingMethod;
        List<TestCase> globalTestCases;
        int lastRunTest;

        public void LoadProblem(string name)
        {
            string problemBody = Path.Combine(problemsRoot, name, "Problem.txt");
            if (!File.Exists(problemBody))
            {
                MessageBox.Show(string.Format("Couldn't open problem description at: {0}", problemBody));
            }

            TrySave();

            try
            {
                using (StreamReader sr = new StreamReader(problemBody))
                {
                    CodingProblem nextProblem = new CodingProblem(name, sr);
                    nextProblem.IsSolved = SolvedProblems.Contains(nextProblem.ProblemId);

                    viewSolutionToolStripMenuItem.Enabled = nextProblem.IsSolved;
                    currentProblem = nextProblem;
                    wbProblemDisplay.DocumentText = MathJax.GetFormattedHtml(currentProblem.ToMathJaxString());
                }

                if (File.Exists(Path.Combine(problemsRoot, name, "SavedAttempt.cs")))
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(problemsRoot, name, "SavedAttempt.cs")))
                    {
                        txtCodeEditor.Text = sr.ReadToEnd();
                    }
                }
                else
                {
                    txtCodeEditor.Text = currentProblem.GenerateMethodStub(problemsRoot);
                }
                rtbOutputDisplay.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("error opening problem description: {0}", e.Message));
            }
        }

        private void LoadProblem_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }

            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi == null)
            {
                Debug.Assert(false);
                return;
            }

            LoadProblem(tsmi.Name);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            programRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (programRoot.StartsWith("file:\\"))
            {
                programRoot = programRoot.Substring(6);
            }

            txtCodeEditor.SetHighlighting("C#");
            problemsRoot = Path.Combine(programRoot, "Problems");

            if (!Directory.Exists(problemsRoot))
            {
                MessageBox.Show(string.Format("Couldn't open problems folder at {0}", problemsRoot));
                Application.Exit();
                return;
            }

            string progressPath = Path.Combine(problemsRoot, "Progress");
            if (File.Exists(progressPath))
            {
                using (StreamReader sr = new StreamReader(progressPath))
                {
                    string s = sr.ReadLine();
                    while (s != null)
                    {
                        int i = Int32.Parse(s);
                        SolvedProblems.Add(i);
                        s = sr.ReadLine();
                    }
                }
            }

            List<ToolStripItem> preSortedItems = new List<ToolStripItem>();
            foreach (var problem in Directory.EnumerateDirectories(problemsRoot))
            {
                ToolStripItem tsi = new ToolStripMenuItem();
                tsi.Text = Path.GetFileName(problem);
                int problemNumber = Int32.Parse(tsi.Text.Split('-')[0].Trim());
                if (SolvedProblems.Contains(problemNumber))
                {
                    tsi.Text += " [SOLVED]";
                }

                tsi.Name = Path.GetFileName(problem);
                tsi.Click += new EventHandler(LoadProblem_Click);
                preSortedItems.Add(tsi);
            }

            preSortedItems.Sort((ToolStripItem first, ToolStripItem second) =>
            {
                int firstProblemNumber = Int32.Parse(first.Text.Split('-')[0].Trim());
                int secondProblemNumber = Int32.Parse(second.Text.Split('-')[0].Trim());
                return firstProblemNumber.CompareTo(secondProblemNumber);
            });

            foreach (var item in preSortedItems)
            {
                problemsToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private MethodInfo CompileCode(string source)
        {
            if (null == currentProblem)
            {
                return null;
            }
            rtbOutputDisplay.Clear();
            rtbOutputDisplay.Text = "Compiling code ... ";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingClass);
            options.ReferencedAssemblies.Add("System.dll");
            var results = provider.CompileAssemblyFromSource(options, new[] { source });
            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    rtbOutputDisplay.AppendText("\n");
                    rtbOutputDisplay.AppendText(error.ToString(), Color.Red, true);
                }
                rtbOutputDisplay.AppendText(string.Format("\n{0} errors encountered. Compile time {1} seconds.", results.Errors.Count, (double)sw.ElapsedMilliseconds / 1000), Color.Red, true);
                return null;
            }

            sw.Stop();
            rtbOutputDisplay.AppendText(string.Format("compilation done in {0} seconds.", (double)sw.ElapsedMilliseconds / 1000));

            var t = results.CompiledAssembly.GetType("MySolution");
            var method = null != t ? t.GetMethod(currentProblem.ProblemName) : null;
            if (null == method)
            {
                rtbOutputDisplay.AppendText("\n");
                rtbOutputDisplay.AppendText(string.Format("Error: No method MySolution::{0} found! Your solution must be a public static method named {0} in a class named MySolution.", currentProblem.ProblemName), Color.Red, true);
                return null;
            }

            var parama = method.GetParameters();
            var retval = method.ReturnType;

            if (parama.Count() != 1 || parama[0].ParameterType != currentProblem.InputParameter
                || retval != currentProblem.OutputParameter)
            {
                rtbOutputDisplay.AppendText("\n");
                rtbOutputDisplay.AppendText(string.Format("Error: MySolution::{0} has incorrect method signature! Method must accept one parameter of type {1} and return {2}.", currentProblem.ProblemName, currentProblem.InputParameter.ToString(), currentProblem.OutputParameter.ToString()), Color.Red, true);
                return null;
            }

            return method;
        }

        private void RunAgainstSample()
        {
            txtCodeEditor.IsReadOnly = true;
            MethodInfo method = CompileCode(txtCodeEditor.Text);
            if (method == null)
            {
                txtCodeEditor.IsReadOnly = false;
                return;
            }

            try
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nTrying method MySolution::{0} with sample input {1}", currentProblem.ProblemName, currentProblem.SampleInputString));

                waitingForResults = true;

                ThreadingHelper threadHelper = new ThreadingHelper();
                threadHelper.RunMethodAsync(delegate
                {
                    try
                    {
                        return method.Invoke(null, new object[] { currentProblem.SampleInput });
                    }
                    catch (ThreadAbortException)
                    {
                        // Thread timeout
                        throw;
                    }
                    catch (Exception e)
                    {
                        if (e.InnerException != null)
                        {
                            rtbOutputDisplay.AppendText(string.Format("\r\nAn exception was thrown executing your method: {0}", e.InnerException.Message), Color.Red, true);
                        }
                        else
                        {
                            rtbOutputDisplay.AppendText(string.Format("\r\nAn exception was thrown executing your method: {0}", e.Message), Color.Red, true);
                        }

                        return null;
                    }
                }, SampleCaseCallback, ProgramTimeout);

                return;
            }
            catch (Exception ex)
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nAn error occurred executing your method: {0}", ex.Message), Color.Red, true);
            }
            txtCodeEditor.IsReadOnly = false;
        }

        public void ProgramTimeout(long elapsedMilliseconds)
        {
            if (globalTestCases == null)
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nYour method timed out after {0} seconds", (double)elapsedMilliseconds / 1000), Color.Red, true);
            }
            else
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nYour method timed out after {0} seconds on the following test case:\r\n", (double)elapsedMilliseconds / 1000), Color.Red, true);
                rtbOutputDisplay.AppendText(globalTestCases[lastRunTest].ExpectedOutput.ToString(), Color.Red, true);
            }
            waitingForResults = false;
            txtCodeEditor.IsReadOnly = false;
        }

        public void SampleCaseCallback(long elapsedMilliseconds, object result)
        {
            rtbOutputDisplay.AppendText(string.Format("\r\n\r\nThe expected output was: {0}", currentProblem.SampleOutputString), Color.Black, true);
            rtbOutputDisplay.AppendText(string.Format("\r\nThe output from your method was: {0}", result == null ? "(null)" : result.ToString()), Color.Black, true);
            if ((currentProblem.OutputParameter == typeof(double) && ((double)result).ApproximatelyEqual((double)currentProblem.SampleOutput)) ||
                currentProblem.SampleOutput.Equals(result))
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nCongratulations, your method passed the sample case! [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.DarkGreen, true);
            }
            else
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nSorry, your method did not pass the sample case. [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.Red, true);
            }
            waitingForResults = false;
            txtCodeEditor.IsReadOnly = false;
        }

        private void btnRunOnSample_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }
            RunAgainstSample();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by uncanardnoir (Github) / 4A18B156 (Reddit).\r\n\r\nThis program and all accompanying test files are made freely available with no warranty under the MIT License (https://opensource.org/licenses/MIT)");
        }

        private void btnClearSolution_Click(object sender, EventArgs e)
        {
            if (waitingForResults || null == currentProblem)
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("This will clear your solution. Are you sure?", "Clear Solution", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                if (currentProblem != null)
                {
                    txtCodeEditor.Text = currentProblem.GenerateMethodStub(problemsRoot);
                }
                else
                {
                    txtCodeEditor.Text = string.Empty;
                }
                rtbOutputDisplay.Clear();
            }

        }

        private void TrySave()
        {
            if (currentProblem != null)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(problemsRoot, currentProblem.DirectoryPath, "SavedAttempt.cs")))
                {
                    sw.Write(txtCodeEditor.Text);
                }
                rtbOutputDisplay.AppendText(string.Format("\r\nFile saved at {0}.", DateTime.Now));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RunAgainstSample();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                TrySave();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }

            waitingForResults = true;
            txtCodeEditor.IsReadOnly = true;

            MethodInfo method = CompileCode(txtCodeEditor.Text);
            if (method == null)
            {
                txtCodeEditor.IsReadOnly = false;
                waitingForResults = false;
                return;
            }

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                MethodInfo solutionMethod;
                using (StreamReader sr = new StreamReader(Path.Combine(problemsRoot, currentProblem.DirectoryPath, "Solution.cs")))
                {
                    solutionMethod = CompileCode(sr.ReadToEnd());
                }
                if (solutionMethod == null)
                {
                    Debug.Assert(false);
                    rtbOutputDisplay.AppendText("Error: couldn't find solution code", Color.Red, true);
                    waitingForResults = false;
                    txtCodeEditor.IsReadOnly = false;
                    return;
                }

                rtbOutputDisplay.AppendText("\r\nGenerating test cases, please wait... ");

                List<TestCase> testCases;
                string testCasesFile = Path.Combine(problemsRoot, currentProblem.DirectoryPath, "TestCases.txt");
                if (File.Exists(testCasesFile))
                {
                    testCases = TestCasesGenerator.GetTestCasesFromFile(solutionMethod, testCasesFile, currentProblem.InputParameter);
                }
                else if (currentProblem.InputParameter == typeof(Int32[]))
                {
                    testCases = TestCasesGenerator.GenerateIntArray(solutionMethod, currentProblem.MaxInputSize, true, true);
                }
                else if (currentProblem.InputParameter == typeof(Int32))
                {
                    testCases = TestCasesGenerator.GenerateInts(solutionMethod, currentProblem.MaxInputSize);
                }
                else if (currentProblem.InputParameter == typeof(char))
                {
                    testCases = TestCasesGenerator.GenerateChars(solutionMethod);
                }
                else
                {
                    Debug.Assert(false);
                    rtbOutputDisplay.AppendText(string.Format("Error with test case: don't know how to generate test cases for type {0}", currentProblem.InputParameter.Name), Color.Red, true);
                    waitingForResults = false;
                    txtCodeEditor.IsReadOnly = false;
                    return;
                }
                sw.Stop();
                rtbOutputDisplay.AppendText(string.Format(" generated in {0} seconds.\r\n", (double)sw.ElapsedMilliseconds / 1000));

                currentTestingMethod = method;
                globalTestCases = testCases;
                lastRunTest = 0;

                ThreadingHelper th = new ThreadingHelper();
                th.RunMethodAsync(delegate
                {
                    try
                    {
                        return currentTestingMethod.Invoke(null, new object[] { globalTestCases[lastRunTest].input });
                    }
                    catch (ThreadAbortException)
                    {
                        // Thread timeout
                        throw;
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            rtbOutputDisplay.AppendText(string.Format("\r\nAn exception was thrown executing your method: {0}", ex.InnerException.Message), Color.Red, true);
                        }
                        else
                        {
                            rtbOutputDisplay.AppendText(string.Format("\r\nAn exception was thrown executing your method: {0}", ex.Message), Color.Red, true);
                        }
                        return null;
                    }
                }, TestInProgressCallback, ProgramTimeout);
            }
            catch (Exception ex)
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nAn error occurred executing your program: {0}", ex.Message), Color.Red, true);
                globalTestCases = null;
                waitingForResults = false;
                txtCodeEditor.IsReadOnly = false;
            }
        }

        void TestInProgressCallback(long elapsedMilliseconds, object result)
        {
            TestCase tc = globalTestCases[lastRunTest];

            rtbOutputDisplay.InvokeAppendText(string.Format("\r\nTest case {0}: ", lastRunTest));
            if ((currentProblem.OutputParameter == typeof(double) && ((double)result).ApproximatelyEqual((double)tc.ExpectedOutput)) ||
                tc.ExpectedOutput.Equals(result))
            {
                rtbOutputDisplay.AppendText(string.Format("PASS [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.DarkGreen, true);
            }
            else
            {
                rtbOutputDisplay.AppendText(string.Format("\r\nThe input was: {0}", tc.input.GetType() == typeof(Int32[]) ? string.Join(",", (Int32[])tc.input) : tc.input.ToString()), Color.Black, true);
                rtbOutputDisplay.AppendText(string.Format("\r\nThe expected output was: {0}", tc.ExpectedOutput.ToString()), Color.Black, true);
                rtbOutputDisplay.AppendText(string.Format("\r\nThe output from your method was: {0}", result == null ? "(null)" : result.ToString()), Color.Black, true);
                rtbOutputDisplay.AppendText(string.Format("\r\nSorry, your method did not pass this test case. [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.Red, true);
                waitingForResults = false;
                txtCodeEditor.IsReadOnly = false;
                globalTestCases = null;
                return;
            }

            lastRunTest++;
            if (lastRunTest < globalTestCases.Count)
            {
                // Recursively kick off the next one
                ThreadingHelper th = new ThreadingHelper();
                th.RunMethodAsync(delegate
                {
                    return currentTestingMethod.Invoke(null, new object[] { globalTestCases[lastRunTest].input });
                }, TestInProgressCallback, ProgramTimeout);
                return;
            }

            // Passing!
            rtbOutputDisplay.AppendText("\r\n\r\nCongratulations, your method passed all test cases!", Color.DarkGreen, true);
            tada.Play();
            if (!currentProblem.IsSolved)
            {
                viewSolutionToolStripMenuItem.InvokeEnable(true);
                currentProblem.IsSolved = true;
                using (StreamWriter strw = new StreamWriter(Path.Combine(problemsRoot, "Progress"), true))
                {
                    strw.WriteLine(currentProblem.ProblemId);
                }
                SolvedProblems.Add(currentProblem.ProblemId);
                foreach (var item in problemsToolStripMenuItem.DropDownItems)
                {
                    ToolStripMenuItem tsmi = item as ToolStripMenuItem;
                    if (Int32.Parse(tsmi.Text.Split('-')[0].Trim()) == currentProblem.ProblemId)
                    {
                        tsmi.InvokeAppendText(" [SOLVED]");
                    }
                }
            }
            globalTestCases = null;
            waitingForResults = false;
            txtCodeEditor.IsReadOnly = false;
        }

        private void progressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("You solved {0} out of {1} problems.", SolvedProblems.Count(), problemsToolStripMenuItem.DropDownItems.Count));
        }

        private void viewSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(currentProblem.IsSolved);
            Process.Start("notepad.exe", Path.Combine(problemsRoot, currentProblem.DirectoryPath, "Solution.cs"));
        }
    }
}
