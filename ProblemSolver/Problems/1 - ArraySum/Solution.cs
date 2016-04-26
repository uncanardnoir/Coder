public class MySolution {
    public static int ArraySum(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++) {
        	sum += input[i];
        }
        return sum;
    }
}