public class MySolution {
    public static bool IsTriangular(int input)
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

    public static bool TriangleNumbers3(int[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (!IsTriangular(input[i]))
            {
                return false;
            }
        }
        return true;
    }
}