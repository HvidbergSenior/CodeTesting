namespace deftq.BuildingBlocks.ExtensionMethods
{
    public static class SubstringOrLessExtension
    {
        public static string SubstringOrLess(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (str.Length >= length)
            {
                return str.Substring(0, length);
            }

            return str;
        }
    }
}
