public class MySolution {
    public static bool PerfectSquares(int input) {
        for (int i = 1; i * i <= input; i++)
        {
            if (i * i == input)
            {
                return true;
            }
        }
        return false;
    }
}