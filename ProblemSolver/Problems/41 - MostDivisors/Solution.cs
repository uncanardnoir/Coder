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

    public static int MostDivisors(int input) {
        int answer = 0;
        for (int i = 1; i <= input; i++)
        {
            if (Phi(i) > answer)
            {
                answer = Phi(i);
            }
        }
        return answer;
    }
}