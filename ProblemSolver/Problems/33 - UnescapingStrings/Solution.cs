public class MySolution {
    public static string UnescapingStrings(string input) {
        string output = string.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '\\')
            {
                switch (input[i + 1])
                {
                    case '"':
                        output += '"';
                        break;
                    case '\'':
                        output += '\'';
                        break;
                    case 't':
                        output += '\t';
                        break;
                    case '?':
                        output += '?';
                        break;
                    case '\\':
                        output += '\\';
                        break;
                    default:
                        break;
                }
                i++;
            }
            else
            {
                output += input[i];
            }
        }
        return output;
    }
}