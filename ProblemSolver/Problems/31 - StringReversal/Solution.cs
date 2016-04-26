public class MySolution {
    public static string StringReversal(string input) {
        string output = string.Empty;
        for (int i = input.Length - 1; i >= 0; i--)
        {
            output += input[i];
        }
        return output;
    }
}