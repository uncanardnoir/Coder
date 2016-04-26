public class MySolution {
    public static bool CountsVsSumRedux(int[] input) {
        int sum = 0;
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] % 2 == 0)
            {
                sum += input[i];
                count++;
            }
        }
        if (sum > count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}