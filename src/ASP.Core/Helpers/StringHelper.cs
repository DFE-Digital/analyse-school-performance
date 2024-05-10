namespace ASP.Core.Helpers
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
    }
}
