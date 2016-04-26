public class MySolution {
    public static bool PositiveCounts(int[] input) {
        int positiveCounts = 0;
        int negativeCounts = 0;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] > 0)
            {
                positiveCounts++;
            }
            else if (input[i] < 0)
            {
                negativeCounts++;
            }
        }

        if (positiveCounts > negativeCounts)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}