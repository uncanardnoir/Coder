public class MySolution {
    public static int AtoI2(string input)
    {
        int answer = 0;
        for (int i = 0; i < input.Length; i++)
        {
            // Push all the existing digits over by 1 place
            answer *= 10;

            // Add the last digit
            answer += input[i] - '0';
        }
        return answer;
    }
}