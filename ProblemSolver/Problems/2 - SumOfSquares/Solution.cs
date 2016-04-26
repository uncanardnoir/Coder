public class MySolution {
    public static int SumOfSquares(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++) {
        	sum += (input[i] * input[i]);
        }
        return sum;
    }
}