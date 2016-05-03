public class MySolution {
    public static int AtoIExample(char input)
    {
        // If the character is either past the end of the alphabet, or before 
        // the beginning of the alphabet, it is invalid and we return -1
        if (input > 'Z' || input < 'A')
        {
            return -1;
        }

        return input - 'A';
    }
}