using Microsoft.CSharp;
using System;
using System.Collections.Generic;
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

        public override string ToString() {
            return string.Format("Problem {0}: {1}\r\n\r\n{2}\r\n\r\nSample Input:\r\n{3}\r\n\r\nSample Output:\r\n{4}\r\n\r\nYour method must be public static named {1} in a class called MySolution.", ProblemId, ProblemName, ProblemDescription, SampleInputString, SampleOutputString);
        }

        public string ToMathJaxString()
        {
            return string.Format("<b>Problem {0}: {1}</b><br/><br/>{2}<br/><b>Sample Input:</b><br/>{3}<br/><br/><b>Sample Output:</b><br/>{4}</br></br>Your method must be public static named {1} in a class called MySolution.", ProblemId, ProblemName, ProblemDescription.Replace("\r\n", "<br />"), SampleInputString, SampleOutputString);
        }

        public string GenerateMethodStub()
        {
            var compiler = new CSharpCodeProvider();
            string outparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(outputParameter));
            string inparam = compiler.GetTypeOutput(new System.CodeDom.CodeTypeReference(inputParameter));
            string defaultValue = string.Empty;
            if (outparam.Equals("bool"))
            {
                defaultValue = " false";
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
