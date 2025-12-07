namespace CDOWin.Extensions;

static class StringExtensions {
    /// <summary>
    /// Normalizes line ending to \n and trims trailing whitespace.
    /// This is useful for multi-line TextBox comparisons to their original
    /// values stored in the model.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string? NormalizeString(this string s) {
        if (s == null) return null;
        return s.Replace("\r\n", "\n").Replace("\r", "\n").TrimEnd();
    }
}
