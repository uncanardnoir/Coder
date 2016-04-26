public class MySolution {
    public static int StarryNumbers(int[] input) {
        int count = 0;
        for (int i = 0; i < input.Length; i++) {
            // Divisible by 3, but NOT by 7 and 11.
            if (input[i] % 3 == 0 && input[i] % 7 != 0 && input[i] % 11 != 0)
            {
                count++;
            }
        }
        return count;
    }
}