public class MySolution {
    public static int EvenCounts(int[] input) {
        int count = 0;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] % 2 == 0)
            {
                count++;
            }
        }
        return count;
    }
}