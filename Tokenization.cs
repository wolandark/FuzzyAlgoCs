public static class TokenizedFuzzySearch
{
    /// <summary>
    /// Pre-processes a list of strings for faster fuzzy searching by tokenizing them
    /// </summary>
    public static Dictionary<string, string[]> CreateTokenIndex(IEnumerable<string> strings)
    {
        return strings.ToDictionary(
            s => s,
            s => s.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                  .Select(t => t.ToLowerInvariant())
                  .ToArray());
    }

    /// <summary>
    /// Finds matches using tokenized comparison (faster but potentially less accurate)
    /// </summary>
    public static IEnumerable<string> FindMatches(
        string input, 
        Dictionary<string, string[]> tokenIndex, 
        double threshold = 0.7)
    {
        if (string.IsNullOrWhiteSpace(input) || tokenIndex == null || tokenIndex.Count == 0)
            return Enumerable.Empty<string>();

        var inputTokens = input.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(t => t.ToLowerInvariant())
                               .ToArray();

        return tokenIndex
            .Select(kvp => new
            {
                Candidate = kvp.Key,
                Score = CalculateTokenSimilarity(inputTokens, kvp.Value)
            })
            .Where(x =xkil> x.Score >= threshold)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Candidate);
    }

    private static double CalculateTokenSimilarity(string[] inputTokens, string[] candidateTokens)
    {
        // Simple Jaccard similarity
        var intersection = inputTokens.Intersect(candidateTokens).Count();
        var union = inputTokens.Union(candidateTokens).Count();
        
        if (union == 0) return 0;
        
        return (double)intersection / union;
    }
}x
