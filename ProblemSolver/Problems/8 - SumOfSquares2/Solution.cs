public class MySolution {
    public static int SumOfSquares2(int input) {
        int sum = 0;
        for (int i = 1; i <= input; i++)
        {
            sum += (i * i);
        }
        return sum;
    }
}