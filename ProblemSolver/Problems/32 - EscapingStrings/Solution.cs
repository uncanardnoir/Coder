public class MySolution {
    public static string EscapingStrings(string input) {
        string output = string.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case '"':
                    output += "\\\"";
                    break;
                case '\'':
                    output += "\\'";
                    break;
                case '?':
                    output += "\\?";
                    break;
                case '\t':
                    output += "\\t";
                    break;
                case '\\':
                    output += "\\\\";
                    break;
                default:
                    output += input[i];
                    break;
            }
        }
        return output;
    }
}