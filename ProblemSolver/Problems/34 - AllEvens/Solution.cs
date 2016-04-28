public class MySolution {
    public static bool AllEvens(int[] input) {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] % 2 == 1)
            {
                return false;
            }
        }
        return true;
    }
}