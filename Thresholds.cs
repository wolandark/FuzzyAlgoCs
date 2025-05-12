public static class FuzzyMatcher
{
    /// <summary>
    /// Finds the best match for a given input string from a list of candidates
    /// </summary>
    /// <param name="input">The string to match</param>
    /// <param name="candidates">List of candidate strings</param>
    /// <param name="threshold">Minimum similarity score (0-1) to consider a match</param>
    /// <returns>The best matching string or null if no match meets the threshold</returns>
    public static string? FindBestMatch(string input, IEnumerable<string> candidates, double threshold = 0.7)
    {
        if (string.IsNullOrWhiteSpace(input) || candidates == null || !candidates.Any())
            return null;

        var bestMatch = candidates
            .Select(c => new { Candidate = c, Score = FuzzySearch.LevenshteinSimilarity(input, c) })
            .Where(x => x.Score >= threshold)
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        return bestMatch?.Candidate;
    }

    /// <summary>
    /// Finds all matches above a certain similarity threshold
    /// </summary>
    public static IEnumerable<string> FindAllMatches(string input, IEnumerable<string> candidates, double threshold = 0.7)
    {
        if (string.IsNullOrWhiteSpace(input) || candidates == null || !candidates.Any())
            return Enumerable.Empty<string>();

        return candidates
            .Select(c => new { Candidate = c, Score = FuzzySearch.LevenshteinSimilarity(input, c) })
            .Where(x => x.Score >= threshold)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Candidate);
    }
}
