public class MySolution {
    public static int MarketTrends(int[] input) {
        int nBullYears = 0;
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] > input[i - 1])
            {
                nBullYears++;
            }
        }
        return nBullYears;
    }
}