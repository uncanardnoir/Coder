public class MySolution {
    public static int NumberOfDivisors(int input) {
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
}