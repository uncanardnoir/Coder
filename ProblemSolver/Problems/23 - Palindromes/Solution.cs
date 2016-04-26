public class MySolution {
    public static bool Palindromes(string input) {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] != input[input.Length - 1 - i])
            {
                return false;
            }
        }
        return true;
    }
}