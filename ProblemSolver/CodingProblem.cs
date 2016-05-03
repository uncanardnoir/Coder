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
        public string DirectoryPath;
        public int ProblemId;
        public string ProblemName;
        public string ProblemDescription;
        public Type inputParameter;
        public int maxInputSize;
        public Type outputParameter;
        public string SampleInputString;
        public string SampleOutputString;
        public object SampleInput;
        public object SampleOutput;
        public bool IsSolved;
        public bool IsExample;

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
                string outparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(outputParameter));
                string inparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(inputParameter));
                string defaultValue = " null";
                if (outparam.Equals("bool"))
                {
                    defaultValue = " false";
                }
                else if (outparam.Equals("char"))
                {
                    defaultValue = " '\\0'";
                }
                else if (outputParameter.IsValueType)
                {
                    defaultValue = string.Format(" {0}", Activator.CreateInstance(outputParameter));
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
