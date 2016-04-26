public class MySolution {
    public static bool Pangrams(string input) {
        bool[] letterSeen = new bool[26];
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == ' ')
            {
                continue;
            }
            if (input[i] >= 'a' && input[i] <= 'z')
            {
                letterSeen[input[i] - 'a'] = true;
            }
            if (input[i] >= 'A' && input[i] <= 'Z')
            {
                letterSeen[input[i] - 'A'] = true;
            }
        }

        for (int i = 0; i < letterSeen.Length; i++)
        {
            if (letterSeen[i] == false)
            {
                return false;
            }
        }
        return true;
    }
}