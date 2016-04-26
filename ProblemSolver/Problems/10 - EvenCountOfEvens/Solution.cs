public class MySolution {
    public static int EvenCountOfEvens(int[] input) {
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if ((i + 1) % 2 == 0 && input[i] % 2 == 0)
            count++;
        }
        return count;
    }
}