using System.Reflection;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Helper
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// Gets the application's base directory path using the entry assembly location
        /// </summary>
        /// <returns>The full path to the application's base directory</returns>
        public static string GetApplicationBasePath()
        {
            var assembly = Assembly.GetEntryAssembly()
                           ?? throw new InvalidOperationException("Unable to determine entry assembly");

            return Path.GetDirectoryName(assembly.Location)
                   ?? throw new DirectoryNotFoundException("Unable to determine application base directory");
        }

        /// <summary>
        /// Gets the solution root directory path by searching up the directory tree for a .sln file
        /// </summary>
        /// <returns>The full path to the solution directory</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when solution directory cannot be found</exception>
        public static string GetSolutionDirectoryPath()
        {
            var directory = new DirectoryInfo(GetApplicationBasePath());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory?.FullName
                   ?? throw new DirectoryNotFoundException(
                       "Solution directory not found. Ensure the application is running within the solution directory structure.");
        }

        /// <summary>
        /// Combines the solution directory path with the specified folder path
        /// </summary>
        /// <param name="folderPath">Relative or absolute path to the folder</param>
        /// <returns>The full path to the folder</returns>
        public static string GetSolutionFolderPath(string folderPath)
        {
            // If the path is absolute, use it as-is
            if (Path.IsPathRooted(folderPath))
            {
                return folderPath;
            }

            // Otherwise, combine it with the solution path
            return Path.Combine(GetSolutionDirectoryPath(), folderPath);
        }
    }
}