public class MySolution {
    public static int OddCountOfOdds(int[] input) {
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if ((i + 1) % 2 == 1 && input[i] % 2 == 1)
            {
                count++;
            }
        }
        return count;
    }
}