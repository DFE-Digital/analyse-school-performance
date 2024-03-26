namespace ASP.Web.Services
{
    public class GitCommitHashCurrentVersionProvider : ICurrentVersionProvider
    {
        private readonly string _rootPath;
        public GitCommitHashCurrentVersionProvider(IHostEnvironment hostEnvironment)
        {
            _rootPath = hostEnvironment.ContentRootPath ?? 
                throw new ArgumentNullException(nameof(hostEnvironment));
        }

        
        public string GetCurrentVersion()
        {
            // 'CurrentCommit.txt' is produced by a Post Build Event running 'git rev-parse --short HEAD'
            // The file will have the commit hash for the current branch when you build the solution
            string filePath = Path.Combine(_rootPath, "CurrentCommit.txt");
            
            if (File.Exists(filePath))
                return File.ReadAllText(filePath);

            return string.Empty;
        }
    }
}
