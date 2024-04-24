using System.Reflection;

namespace ASP.Web.Features.ApplicationServiceVersion
{
    public class GitCommitHashCurrentVersionProvider : ICurrentVersionProvider
    {
        private readonly string? _outputDirectory;
        public GitCommitHashCurrentVersionProvider()
        {
            _outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }


        public string GetCurrentVersion()
        {
            if (string.IsNullOrEmpty(_outputDirectory))
                return string.Empty;

            // 'CurrentCommit.txt' is produced by a Post Build Event running 'git rev-parse --short HEAD'
            // The file will have the commit hash for the current branch when you build the solution
            string filePath = Path.Combine(_outputDirectory, "CurrentCommit.txt");

            if (File.Exists(filePath))
                return File.ReadAllText(filePath);

            return string.Empty;
        }
    }
}
