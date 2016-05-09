public class MySolution {
    public static bool TriangleNumbers2(int input)
    {
        int answer = 0;
        for (int i = 1; answer < input; i++)
        {
            answer += i;
        }
        if (answer == input)
        {
            return true;
        }
        return false;
    }
}