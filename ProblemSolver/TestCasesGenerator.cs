using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolver
{
    class TestCase
    {
        public object input;
        public object ExpectedOutput;
        public TestCase(object i, object o)
        {
            input = i;
            ExpectedOutput = o;
        }
    }

    class TestCasesGenerator
    {
        public static List<TestCase> GetTestCasesFromFile(MethodInfo solutionMethod, string testCaseFile, Type inputType)
        {
            List<TestCase> testCases = new List<TestCase>();

            using (StreamReader sr = new StreamReader(testCaseFile))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    object nextInput;
                    if (inputType == typeof(string))
                    {
                        nextInput = s;
                    }
                    else if (inputType == typeof(Int32))
                    {
                        nextInput = Int32.Parse(s);
                    }
                    else
                    {
                        Debug.Assert(false);
                        return null;
                    }
                    object nextOutput = solutionMethod.Invoke(null, new object[] { nextInput });
                    testCases.Add(new TestCase(nextInput, nextOutput));
                }
            }

            return testCases;
        }

        public static List<TestCase> GenerateInts(MethodInfo solutionMethod, int maxLength)
        {
            Debug.Assert(maxLength >= 10);
            List<TestCase> testCases = new List<TestCase>();

            int simple1 = 1;
            object output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = 2;
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = 3;
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = maxLength;
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = maxLength - 1;
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                simple1 = r.Next(maxLength - 5) + 4;
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }

            return testCases;
        }

        public static List<TestCase> GenerateIntArray(MethodInfo solutionMethod, int maxLength, bool canBeZero = true, bool canBeNegative = true)
        {
            Debug.Assert(maxLength >= 10);
            List<TestCase> testCases = new List<TestCase>();

            int[] simple1 = { 2, 4, 6, 8, 10 };
            object output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = new int[] { 1, 3, 5, 7 };
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            simple1 = new int[] { 1, 2, 3, 5, 9, 15, 20 };
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            if (canBeZero)
            {
                simple1 = new int[] { 0 };
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));

                simple1 = new int[] { 0, 0, 0, 0, 0 };
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }


            if (canBeNegative)
            {
                simple1 = new int[] { -1, -3, -5, -8, -12 };
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));

                simple1 = new int[] { 1, -1, 2, -2, 3, -3, 10, -10, 15, 20 };
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }

            if (canBeZero && canBeNegative)
            {
                simple1 = new int[] { -1, 0, 5, 10, -20, -30, 100 };
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }

            Random r = new Random();
            simple1 = new int[maxLength];
            for (int i = 0; i < maxLength; i++)
            {
                simple1[i] = r.Next(100);
            }
            output1 = solutionMethod.Invoke(null, new object[] { simple1 });
            testCases.Add(new TestCase(simple1, output1));

            if (canBeZero)
            {
                simple1 = new int[maxLength];
                for (int i = 0; i < maxLength; i++)
                {
                    if (r.Next(5) == 0)
                    {
                        simple1[i] = 0;
                    }
                    else
                    {
                        simple1[i] = r.Next(100);
                    }
                }
                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }

            if (canBeNegative)
            {
                simple1 = new int[maxLength];
                for (int i = 0; i < maxLength; i++)
                {
                    int next;
                    do
                    {
                        next = r.Next(200) - 100;
                    } while (next == 0 && !canBeZero);
                    simple1[i] = next;
                }

                output1 = solutionMethod.Invoke(null, new object[] { simple1 });
                testCases.Add(new TestCase(simple1, output1));
            }

            return testCases;
        }

        public static object UnmarshalObject(Type t, string obj)
        {
            if (t == typeof(Int32))
            {
                return Int32.Parse(obj);
            }
            else if (t == typeof(Int32[]))
            {
                string[] inpt = obj.Substring(1, obj.Length - 2).Split(',');
                int[] outpt = new int[inpt.Length];
                for (int i = 0; i < inpt.Length; i++)
                {
                    outpt[i] = Int32.Parse(inpt[i].Trim());
                }
                return outpt;
            }
            else if (t == typeof(Boolean))
            {
                return Boolean.Parse(obj);
            }
            else if (t == typeof(string))
            {
                return obj.ToString();
            }
            else if (t == typeof(double))
            {
                return double.Parse(obj);
            }
            else
            {
                Debug.Assert(false);
                return null;
            }
        }
    }

}