public class MySolution {
    public static int TriangleNumbers(int input)
    {
        int answer = 0;
        for (int i = 1; i <= input; i++)
        {
            answer += i;
        }
        return answer;
    }
}