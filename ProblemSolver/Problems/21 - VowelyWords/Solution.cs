public class MySolution {
    public static bool VowelyWords(string input) {
        int vowels = 0;
        int consonants = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 'a' || input[i] == 'A' ||
                input[i] == 'e' || input[i] == 'E' ||
                input[i] == 'i' || input[i] == 'I' ||
                input[i] == 'o' || input[i] == 'O' ||
                input[i] == 'u' || input[i] == 'U')
            {
                vowels++;
            }
            else
            {
                consonants++;
            }
        }
        return (vowels > consonants);
    }
}