public class MySolution {
    public static bool Anagrams(string input) {
        string[] words = input.Split(';');
        int[] letterCounts = new int[26];
        for (int i = 0; i < words[0].Length; i++)
        {
            letterCounts[words[0][i] - 'a']++;
        }

        for (int i = 0; i < words[1].Length; i++)
        {
            letterCounts[words[1][i] - 'a']--;
        }

        for (int i = 0; i < letterCounts.Length; i++)
        {
            if (letterCounts[i] != 0) return false;
        }
        return true;
    }
}