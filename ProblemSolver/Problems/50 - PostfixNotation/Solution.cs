using System.Collections.Generic;

public class MySolution {
    public static int PostfixNotation(string input)
    {
        Stack<int> operandStack = new Stack<int>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] >= '0' && input[i] <= '9')
            {
                operandStack.Push(input[i] - '0');
            }
            else if (input[i] == '+')
            {
                operandStack.Push(operandStack.Pop() + operandStack.Pop());
            }
            else if (input[i] == '-')
            {
                operandStack.Push(-operandStack.Pop() + operandStack.Pop());
            }
            else if (input[i] == '*')
            {
                operandStack.Push(operandStack.Pop() * operandStack.Pop());
            }
        }
        return operandStack.Pop();
    }
}