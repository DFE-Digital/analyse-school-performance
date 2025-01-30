namespace ASP.Core.Text
{
    public static class StringHelper
    {
        /// <summary>
        /// Concatenates non-empty string representations of objects using a specified separator.
        /// </summary>
        /// <param name="separator">The separator to place between each non-empty string representation.</param>
        /// <param name="values">The objects whose string representations will be concatenated.</param>
        /// <returns>A single string containing non-empty string representations of the provided objects, separated by the specified separator.</returns>
        public static string ConcatNonEmpties(string separator, params object[] values)
        {
            return string.Join(separator, values.Where(x => x is not null && !string.IsNullOrEmpty(x.ToString())));
        }

        /// <summary>
        /// Removes the given text from the start of a string, if present
        /// </summary>
        /// <param name="textToRemove">Text to remove from the target string</param>
        /// <param name="value">Target string to remove text from</param>
        /// <returns>The original string with the given text removed from the start</returns>
        public static string RemoveFromStart(string textToRemove, string value)
        {
            if (!value.StartsWith(textToRemove))
            {
                return value;
            }

            return value.Substring(textToRemove.Length);
        }
    }
}
