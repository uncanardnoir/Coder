public class MySolution {
    public static int AtoI(char input)
    {
        if (input > '9' || input < '0')
        {
            return -1;
        }

        return input - '0';
    }
}