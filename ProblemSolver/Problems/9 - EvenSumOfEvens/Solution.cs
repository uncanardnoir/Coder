public class MySolution {
    public static int EvenSumOfEvens(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if ((i + 1) % 2 == 0 && input[i] % 2 == 0)
            sum += input[i];
        }
        return sum;
    }
}