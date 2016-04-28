public class MySolution {
    public static bool AllOdds(int[] input) {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] % 2 == 0)
            {
                return false;
            }
        }
        return true;
    }
}