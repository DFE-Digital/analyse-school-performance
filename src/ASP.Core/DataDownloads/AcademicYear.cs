namespace ASP.Core.DataDownloads
{
    public class AcademicYear
    {
        public int Year { get; set; }
        public string StartToEndYears { get; set; } = "";

        /// <summary>
        /// Converts a list of years into a list of AcademicYear objects with formatted year ranges.
        /// </summary>
        /// <param name="years">List of academic end years</param>
        /// <returns>List of AcademicYear objects with formatted year ranges</returns>
        public static List<AcademicYear> ToAcademicYears(IEnumerable<int> years)
        {
            if (years == null)
                throw new ArgumentNullException(nameof(years));

            return years
                .Distinct()
                .Select(year => new AcademicYear {
                    Year = year,
                    StartToEndYears = $"{year - 1} to {year}"
                })
                .ToList();
        }
    }
}
