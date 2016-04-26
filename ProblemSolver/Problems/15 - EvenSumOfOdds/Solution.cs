public class MySolution {
    public static int EvenSumOfOdds(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if ((i + 1) % 2 == 0 && input[i] % 2 == 1)
            {
                sum += input[i];
            }
        }
        return sum;
    }
}