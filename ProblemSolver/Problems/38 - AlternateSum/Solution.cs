public class MySolution {
    public static int AlternateSum(int[] input) {
        int answer = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0)
            {
                answer += input[i];
            }
            else
            {
                answer -= input[i];
            }
        }
        return answer;
    }
}