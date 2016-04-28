public class MySolution
{
    public static int Palindromes2(string input)
    {
        int count = 0;
        for (int i = 0; i < input.Length; i++) {
            for (int j = i + 1; j < input.Length + 1; j++) {
                string temp = input.Substring(i, j - i);
                bool fFoundPalindrome = true;
                for (int k = 0; k < temp.Length; k++) {
                    if (temp.Length <= 1) {
                        fFoundPalindrome = false;
                        break;
                    }
                    if (temp[k] != temp[temp.Length - 1 - k]) {
                        fFoundPalindrome = false;
                        break;
                    }
                }
                if (fFoundPalindrome) {
                    count++;
                }
            }
        }
        return count;
    }
}