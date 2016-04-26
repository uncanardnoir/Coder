public class MySolution {
    public static int Collatz(int input) {
        int numberOfSteps = 0;
        while (input != 1)
        {
            if (input % 2 == 0)
            {
                input /= 2;
            }
            else
            {
                input = 3 * input + 1;
            }
            numberOfSteps++;
        }

        return numberOfSteps;
    }

    public static int Collatz2(int input)
    {
        int answer = 1;
        int longestLength = 1;
        for (int i = 2; i <= input; i++)
        {
            int collatzLength = Collatz(i);
            if (collatzLength > longestLength)
            {
                answer = i;
                longestLength = collatzLength;
            }
        }
        return answer;
    }
}