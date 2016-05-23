public class MySolution {
    public static int PriceIsRight2(int[] input)
    {
        int winningGuess = -1;
        int winningContestant = -1;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] > winningGuess && input[i] <= 1000) {
                winningGuess = input[i];
                winningContestant = i;
            }
        }
        return winningContestant;
    }
}