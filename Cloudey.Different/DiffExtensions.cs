using Cloudey.Different.DiffMatchPatch;

namespace Cloudey.Different;

public static class DiffExtensions
{
    private static diff_match_patch _dmi = new();

    public static List<Diff> Diff (this string from, string to)
    {
        return _dmi.diff_main(from, to);
    }

    public static List<Diff> SemanticDiff (this string from, string to)
    {
        var diffs = from.Diff(to);
        _dmi.diff_cleanupSemantic(diffs);
        return diffs;
    }

    public static List<Diff> EfficientDiff (this string from, string to)
    {
        var diffs = from.Diff(to);
        _dmi.diff_cleanupEfficiency(diffs);
        return diffs;
    }

    public static int Levenshtein (this List<Diff> diffs)
    {
        return _dmi.diff_levenshtein(diffs);
    }

    public static string Html (this List<Diff> diffs)
    {
        return _dmi.diff_prettyHtml(diffs);
    }
}