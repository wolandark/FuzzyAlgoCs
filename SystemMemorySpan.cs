public static class FastFuzzySearch
{
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        if (a.IsEmpty || b.IsEmpty)
            return a.IsEmpty && b.IsEmpty ? 1.0 : 0.0;

        if (a.SequenceEqual(b)) return 1.0;

        int maxLen = Math.Max(a.Length, b.Length);
        int distance = LevenshteinDistance(a, b);
        return 1.0 - (double)distance / maxLen;
    }

    private static int LevenshteinDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        // Optimized for memory efficiency using two single-dimensional arrays
        var previousRow = new int[b.Length + 1];
        var currentRow = new int[b.Length + 1];

        for (int i = 0; i < previousRow.Length; i++)
            previousRow[i] = i;

        for (int i = 0; i < a.Length; i++)
        {
            currentRow[0] = i + 1;

            for (int j = 0; j < b.Length; j++)
            {
                int cost = (a[i] == b[j]) ? 0 : 1;
                currentRow[j + 1] = Math.Min(
                    Math.Min(currentRow[j] + 1, previousRow[j + 1] + 1),
                    previousRow[j] + cost);
            }

            (previousRow, currentRow) = (currentRow, previousRow);
        }

        return previousRow[b.Length];
    }
}
