public class MySolution {
    public static int SumOfEvens(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] % 2 == 0)
            {
                sum += input[i];
            }
        }
        return sum;
    }
}