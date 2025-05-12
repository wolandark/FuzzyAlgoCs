public static class FuzzySearch
{
    /// <summary>
    /// Computes the Levenshtein distance between two strings.
    /// This is the number of edits needed to transform one string into the other.
    /// </summary>
    public static int LevenshteinDistance(string a, string b)
    {
        if (string.IsNullOrEmpty(a))
            return string.IsNullOrEmpty(b) ? 0 : b.Length;
        
        if (string.IsNullOrEmpty(b))
            return a.Length;

        // Create distance matrix
        int[,] distance = new int[a.Length + 1, b.Length + 1];

        // Initialize first row and column
        for (int i = 0; i <= a.Length; distance[i, 0] = i++) { }
        for (int j = 0; j <= b.Length; distance[0, j] = j++) { }

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                
                distance[i, j] = Math.Min(
                    Math.Min(distance[i - 1, j] + 1,     // Deletion
                    distance[i, j - 1] + 1),            // Insertion
                    distance[i - 1, j - 1] + cost);     // Substitution
            }
        }

        return distance[a.Length, b.Length];
    }

    /// <summary>
    /// Returns a normalized similarity score between 0 (completely different) and 1 (exact match)
    /// </summary>
    public static double LevenshteinSimilarity(string a, string b)
    {
        if (a == b) return 1.0;
        
        int maxLen = Math.Max(a.Length, b.Length);
        if (maxLen == 0) return 1.0; // both empty
        
        int distance = LevenshteinDistance(a, b);
        return 1.0 - (double)distance / maxLen;
    }
}
