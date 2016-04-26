public class MySolution {
    public static bool CountsVsSum(int[] input) {
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            sum += input[i];
        }
        if (sum > input.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}