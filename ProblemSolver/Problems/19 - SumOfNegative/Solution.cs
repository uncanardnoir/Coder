public class MySolution {
    public static int SumOfNegative(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] < 0)
            {
                sum += input[i];
            }
        }
        return sum;
    }
}