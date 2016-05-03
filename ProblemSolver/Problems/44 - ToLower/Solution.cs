public class MySolution {
    public static char ToLower(char input)
    {
        if (input > 'Z' || input < 'A')
        {
            return input;
        }

        return (char)(input - 'A' + 'a');
    }
}