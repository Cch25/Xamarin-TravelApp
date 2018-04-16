namespace RentFlixApp.Extensions
{
    public static class ExtensionsMethods
    {
        public static string SafeSubstring(this string text, int start, int length)
        {
            return text.Length <= start ? ""
                : text.Length - start <= length ? text.Substring(start)
                : text.Substring(start, length);
        }
    }
}
