using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolver
{
    class CodingProblem {
        public string DirectoryPath { get; private set; }
        public int ProblemId { get; private set; }
        public string ProblemName { get; private set; }
        public string ProblemDescription { get; private set; }
        public Type InputParameter { get; private set; }
        public int MaxInputSize { get; private set; }
        public Type OutputParameter { get; private set; }
        public string SampleInputString { get; private set; }
        public string SampleOutputString { get; private set; }
        public object SampleInput { get; private set; }
        public object SampleOutput { get; private set; }
        public bool IsSolved { get; set; }
        public bool IsExample { get; private set; }

        public CodingProblem(string name, StreamReader sr)
        {
            string[] nameSplit = name.Split('-');
            DirectoryPath = name;
            ProblemId = int.Parse(nameSplit[0].Trim());
            ProblemName = nameSplit[1].Trim();
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
                ProblemDescription = sb.ToString();
            }
            InputParameter = Type.GetType(sr.ReadNoncommentLine());
            MaxInputSize = int.Parse(sr.ReadNoncommentLine());
            OutputParameter = Type.GetType(sr.ReadNoncommentLine());
            SampleInputString = sr.ReadNoncommentLine();
            SampleOutputString = sr.ReadNoncommentLine();

            string isExampleInput;
            if ((isExampleInput = sr.ReadNoncommentLine()) != null)
            {
                IsExample = bool.Parse(isExampleInput);
            }

            SampleInput = TestCasesGenerator.UnmarshalObject(InputParameter, SampleInputString);
            SampleOutput = TestCasesGenerator.UnmarshalObject(OutputParameter, SampleOutputString);
        }

        public override string ToString() {
            return string.Format("{5}Problem {0}: {1}\r\n\r\n{2}\r\n\r\nSample Input:\r\n{3}\r\n\r\nSample Output:\r\n{4}\r\n\r\nYour method must be public static named {1} in a class called MySolution.",
                ProblemId,
                ProblemName,
                ProblemDescription,
                SampleInputString,
                SampleOutputString,
                IsExample ? "This is an example problem - it is meant to teach you a new concept by giving you the solution. You can play with the solution and change the code to see how it works. Clearing the solution will reset it back to the orignal solution.\r\n\r\n" : string.Empty);
        }

        public string ToMathJaxString()
        {
            return string.Format("{5}<b>Problem {0}: {1}</b><br/><br/>{2}<br/><b>Sample Input:</b><br/>{3}<br/><br/><b>Sample Output:</b><br/>{4}</br></br>Your method must be public static named {1} in a class called MySolution.",
                ProblemId,
                ProblemName,
                ProblemDescription.Replace("\r\n", "<br />"),
                SampleInputString,
                SampleOutputString,
                IsExample ? "<i>This is an example problem - it is meant to teach you a new concept by giving you the solution. You can play with the solution and change the code to see how it works. Clearing the solution will reset it back to the orignal solution.</i><br /><br />" : string.Empty);
        }

        public string GenerateMethodStub(string problemRoot)
        {
            if (IsExample)
            {
                using (StreamReader sr = new StreamReader(Path.Combine(problemRoot, DirectoryPath, "Solution.cs"))) {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                var compiler = new CSharpCodeProvider();
                string outparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(OutputParameter));
                string inparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(InputParameter));
                string defaultValue = " null";
                if (outparam.Equals("bool"))
                {
                    defaultValue = " false";
                }
                else if (outparam.Equals("char"))
                {
                    defaultValue = " '\\0'";
                }
                else if (OutputParameter.IsValueType)
                {
                    defaultValue = string.Format(" {0}", Activator.CreateInstance(OutputParameter));
                }

                return string.Format("public class MySolution {{\r\n    public static {0} {1}({2} input) {{\r\n        // TODO: Your code here\r\n        return{3};\r\n    }}\r\n}}",
                    outparam,
                    ProblemName,
                    inparam,
                    defaultValue
                    );
            }
        }
    }
}
