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

    public static int Collatz3(int input)
    {
        int numberOfChains = 0;
        for (int i = 1; i <= 200; i++)
        {
            if (Collatz(i) == input)
            {
                numberOfChains++;
            }
        }
        return numberOfChains;
    }
}