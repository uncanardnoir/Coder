public class MySolution {
    public static bool Primes(int input) {
        if (input < 2) {
            return false;
        }

        for (int i = 2; i * i <= input; i++)
        {
            if (input % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}