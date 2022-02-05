using Cloudey.Different.DiffMatchPatch;

namespace Cloudey.Different;

public static class PatchExtensions
{
    private static diff_match_patch _dmi = new();
    public static List<Patch> Patch (this string from, string to)
    {
        return _dmi.patch_make(from, to);
    }

    public static List<Patch> Patch (this List<Diff> diffs)
    {
        return _dmi.patch_make(diffs);
    }

    public static List<Patch> Patch (this string from, List<Diff> diffs)
    {
        return _dmi.patch_make(from, diffs);
    }

    public static string Text (this List<Patch> patches)
    {
        return _dmi.patch_toText(patches);
    }

    public static bool TryParsePatches (this string patchesAsText, out List<Patch>? result)
    {
        try
        {
            result = patchesAsText.ParsePatches();
            return true;
        }
        catch
        {
            result = null;
        }

        return false;
    }

    public static List<Patch> ParsePatches (this string patchesAsText)
    {
        return _dmi.patch_fromText(patchesAsText);
    }

    public static string? Apply (this List<Patch> patches, string from)
    {
        return _dmi.patch_apply(patches, from)[0] as string;
    }

    public static string? ApplyPatches (this string from, List<Patch> patches)
    {
        return patches.Apply(from);
    }
}