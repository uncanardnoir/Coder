public class MySolution {
    public static int PriceIsRight(int[] input)
    {
        int winningGuess = -1;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] > winningGuess && input[i] <= 1000) {
                winningGuess = input[i];
            }
        }
        return winningGuess;
    }
}