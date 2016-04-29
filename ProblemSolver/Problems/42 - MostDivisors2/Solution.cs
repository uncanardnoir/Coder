public class MySolution {
    public static int Phi(int input)
    {
        int answer = 0;
        for (int i = 1; i <= input; i++)
        {
            if (input % i == 0)
            {
                answer++;
            }
        }
        return answer;
    }

    public static int MostDivisors2(int input) {
        int largestPhi = 0;
        int answer = 0;
        for (int i = 1; i <= input; i++)
        {
            if (Phi(i) > largestPhi)
            {
                answer = i;
                largestPhi = Phi(i);
            }
        }
        return answer;
    }
}