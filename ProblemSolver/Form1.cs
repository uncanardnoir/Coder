using Microsoft.CSharp;
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
    public partial class Form1 : Form
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
                string[] namesplt = name.Split('-');
                using (StreamReader sr = new StreamReader(problemBody))
                {
                    CodingProblem nextProblem = new CodingProblem();
                    nextProblem.DirectoryPath = name;
                    nextProblem.ProblemId = int.Parse(namesplt[0].Trim());
                    nextProblem.ProblemName = namesplt[1].Trim();
                    {
                        StringBuilder sb = new StringBuilder();
                        do
                        {
                            string s = sr.ReadNoncommentLine();
                            if (!s.Equals("%%%"))
                            {
                                sb.AppendLine(s);
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        nextProblem.ProblemDescription = sb.ToString();
                    }
                    nextProblem.inputParameter = Type.GetType(sr.ReadNoncommentLine());
                    nextProblem.maxInputSize = int.Parse(sr.ReadNoncommentLine());
                    nextProblem.outputParameter = Type.GetType(sr.ReadNoncommentLine());
                    nextProblem.SampleInputString = sr.ReadNoncommentLine();
                    nextProblem.SampleOutputString = sr.ReadNoncommentLine();
                    nextProblem.SampleInput = TestCasesGenerator.UnmarshalObject(nextProblem.inputParameter, nextProblem.SampleInputString);
                    nextProblem.SampleOutput = TestCasesGenerator.UnmarshalObject(nextProblem.outputParameter, nextProblem.SampleOutputString);
                    nextProblem.IsSolved = SolvedProblems.Contains(nextProblem.ProblemId);

                    viewSolutionToolStripMenuItem.Enabled = nextProblem.IsSolved;
                    currentProblem = nextProblem;
                    webBrowser1.DocumentText = MathJax.GetFormattedHtml(currentProblem.ToMathJaxString());
                }

                if (File.Exists(Path.Combine(problemsRoot, name, "SavedAttempt.cs")))
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(problemsRoot, name, "SavedAttempt.cs")))
                    {
                        textBox1.Text = sr.ReadToEnd();
                    }
                }
                else
                {
                    textBox1.Text = currentProblem.GenerateMethodStub();
                }
                richTextBox1.Clear();
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            programRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (programRoot.StartsWith("file:\\"))
            {
                programRoot = programRoot.Substring(6);
            }

            textBox1.SetHighlighting("C#");
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
            richTextBox1.Clear();
            richTextBox1.Text = "Compiling code ... ";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var provider = CSharpCodeProvider.CreateProvider("c#");
            var options = new CompilerParameters();
            var assemblyContainingClass = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add(assemblyContainingClass);
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            var results = provider.CompileAssemblyFromSource(options, new[] { source });
            if (results.Errors.Count > 0)
            {
                foreach (var error in results.Errors)
                {
                    richTextBox1.AppendText("\n");
                    richTextBox1.AppendText(error.ToString(), Color.Red, true);
                }
                richTextBox1.AppendText(string.Format("\n{0} errors encountered. Compile time {1} seconds.", results.Errors.Count, (double)sw.ElapsedMilliseconds / 1000), Color.Red, true);
                return null;
            }

            sw.Stop();
            richTextBox1.AppendText(string.Format("compilation done in {0} seconds.", (double)sw.ElapsedMilliseconds / 1000));

            var t = results.CompiledAssembly.GetType("MySolution");
            var method = null != t ? t.GetMethod(currentProblem.ProblemName) : null;
            if (null == method)
            {
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText(string.Format("Error: No method MySolution::{0} found! Your solution must be a public static method named {0} in a class named MySolution.", currentProblem.ProblemName), Color.Red, true);
                return null;
            }

            var parama = method.GetParameters();
            var retval = method.ReturnType;

            if (parama.Count() != 1 || parama[0].ParameterType != currentProblem.inputParameter
                || retval != currentProblem.outputParameter)
            {
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText(string.Format("Error: MySolution::{0} has incorrect method signature! Method must accept one parameter of type {1} and return {2}.", currentProblem.ProblemName, currentProblem.inputParameter.ToString(), currentProblem.outputParameter.ToString()), Color.Red, true);
                return null;
            }

            return method;
        }

        private void RunAgainstSample()
        {
            textBox1.IsReadOnly = true;
            MethodInfo method = CompileCode(textBox1.Text);
            if (method == null)
            {
                textBox1.IsReadOnly = false;
                return;
            }

            try
            {
                richTextBox1.AppendText(string.Format("\r\nTrying method MySolution::{0} with sample input {1}", currentProblem.ProblemName, currentProblem.SampleInputString));

                waitingForResults = true;

                ThreadingHelper threadHelper = new ThreadingHelper();
                threadHelper.RunMethodAsync(delegate
                {
                    return method.Invoke(null, new object[] { currentProblem.SampleInput });
                }, SampleCaseCallback, ProgramTimeout);

                return;
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(string.Format("\r\nAn error occurred executing your program: {0}", ex.Message), Color.Red, true);
            }
            textBox1.IsReadOnly = false;
        }

        public void ProgramTimeout(long elapsedMilliseconds)
        {
            if (globalTestCases == null)
            {
                richTextBox1.AppendText(string.Format("\r\nYour method timed out after {0} seconds", (double)elapsedMilliseconds / 1000), Color.Red, true);
            }
            else
            {
                richTextBox1.AppendText(string.Format("\r\nYour method timed out after {0} seconds on the following test case:\r\n", (double)elapsedMilliseconds / 1000), Color.Red, true);
                richTextBox1.AppendText(globalTestCases[lastRunTest].ExpectedOutput.ToString(), Color.Red, true);
            }
            waitingForResults = false;
            textBox1.IsReadOnly = false;
        }

        public void SampleCaseCallback(long elapsedMilliseconds, object result)
        {
            richTextBox1.AppendText(string.Format("\r\n\r\nThe expected output was: {0}", currentProblem.SampleOutputString), Color.Black, true);
            richTextBox1.AppendText(string.Format("\r\nThe output from your method was: {0}", result.ToString()), Color.Black, true);
            if (currentProblem.SampleOutput.Equals(result))
            {
                richTextBox1.AppendText(string.Format("\r\nCongratulations, your method passed the sample case! [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.DarkGreen, true);
            }
            else
            {
                richTextBox1.AppendText(string.Format("\r\nSorry, your method did not pass the sample case. [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.Red, true);
            }
            waitingForResults = false;
            textBox1.IsReadOnly = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }
            RunAgainstSample();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Michael Ty - abigbottleoforangina@gmail.com.\n\nThis program and all accompanying test files are made freely available with no warranty under the MIT License (https://opensource.org/licenses/MIT)");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("This will clear your solution. Are you sure?", "Clear Solution", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                if (currentProblem != null)
                {
                    textBox1.Text = currentProblem.GenerateMethodStub();
                }
                else
                {
                    textBox1.Text = string.Empty;
                }
                richTextBox1.Clear();
            }

        }

        private void TrySave()
        {
            if (currentProblem != null)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(problemsRoot, currentProblem.DirectoryPath, "SavedAttempt.cs")))
                {
                    sw.Write(textBox1.Text);
                }
                richTextBox1.AppendText(string.Format("\r\nFile saved at {0}.", DateTime.Now));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (waitingForResults)
            {
                return;
            }

            waitingForResults = true;
            textBox1.IsReadOnly = true;

            MethodInfo method = CompileCode(textBox1.Text);
            if (method == null)
            {
                textBox1.IsReadOnly = false;
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
                    richTextBox1.AppendText("Error: couldn't find solution code", Color.Red, true);
                    waitingForResults = false;
                    textBox1.IsReadOnly = false;
                    return;
                }

                richTextBox1.AppendText("\r\nGenerating test cases, please wait... ");

                List<TestCase> testCases;
                string testCasesFile = Path.Combine(problemsRoot, currentProblem.DirectoryPath, "TestCases.txt");
                if (File.Exists(testCasesFile))
                {
                    testCases = TestCasesGenerator.GetTestCasesFromFile(solutionMethod, testCasesFile, currentProblem.inputParameter);
                }
                else if (currentProblem.inputParameter == typeof(Int32[]))
                {
                    testCases = TestCasesGenerator.GenerateIntArray(solutionMethod, currentProblem.maxInputSize, true, true);
                }
                else if (currentProblem.inputParameter == typeof(Int32))
                {
                    testCases = TestCasesGenerator.GenerateInts(solutionMethod, currentProblem.maxInputSize);
                }
                else
                {
                    Debug.Assert(false);
                    richTextBox1.AppendText(string.Format("Error with test case: don't know how to generate test cases for type {0}", currentProblem.inputParameter.Name), Color.Red, true);
                    waitingForResults = false;
                    textBox1.IsReadOnly = false;
                    return;
                }
                sw.Stop();
                richTextBox1.AppendText(string.Format(" generated in {0} seconds.\r\n", (double)sw.ElapsedMilliseconds / 1000));

                currentTestingMethod = method;
                globalTestCases = testCases;
                lastRunTest = 0;

                ThreadingHelper th = new ThreadingHelper();
                th.RunMethodAsync(delegate
                {
                    return currentTestingMethod.Invoke(null, new object[] { globalTestCases[lastRunTest].input });
                }, TestInProgressCallback, ProgramTimeout);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(string.Format("\r\nAn error occurred executing your program: {0}", ex.Message), Color.Red, true);
                globalTestCases = null;
                waitingForResults = false;
                textBox1.IsReadOnly = false;
            }
        }

        void TestInProgressCallback(long elapsedMilliseconds, object result)
        {
            TestCase tc = globalTestCases[lastRunTest];

            richTextBox1.InvokeAppendText(string.Format("\r\nTest case {0}: ", lastRunTest));
            if (tc.ExpectedOutput.Equals(result))
            {
                richTextBox1.AppendText(string.Format("PASS [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.DarkGreen, true);
            }
            else
            {
                richTextBox1.AppendText(string.Format("\r\nThe input was: {0}", tc.input.GetType() == typeof(Int32[]) ? string.Join(",", (Int32[])tc.input) : tc.input.ToString()), Color.Black, true);
                richTextBox1.AppendText(string.Format("\r\nThe expected output was: {0}", tc.ExpectedOutput.ToString()), Color.Black, true);
                richTextBox1.AppendText(string.Format("\r\nThe output from your method was: {0}", result.ToString()), Color.Black, true);
                richTextBox1.AppendText(string.Format("\r\nSorry, your method did not pass this test case. [{0} seconds]", (double)elapsedMilliseconds / 1000), Color.Red, true);
                waitingForResults = false;
                textBox1.IsReadOnly = false;
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
            richTextBox1.AppendText("\r\n\r\nCongratulations, your method passed all test cases!", Color.DarkGreen, true);
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
            textBox1.IsReadOnly = false;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
