public class MySolution {
    public static int EvenSum(int[] input) {
        int sum = 0;
        for (int i = 1; i < input.Length; i+=2) {
        	sum += input[i];
        }
        return sum;
    }
}