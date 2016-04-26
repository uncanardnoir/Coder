public class MySolution {
    public static int OddSumOfEvens(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if ((i + 1) % 2 == 1 && input[i] % 2 == 0)
            {
                sum += input[i];
            }
        }
        return sum;
    }
}