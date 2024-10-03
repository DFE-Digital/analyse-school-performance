using ASP.Core;
using ASP.Core.Results;
using System.Text;

namespace ASP.Infrastructure.InMemory
{
    public class InMemoryBlobStorage : IBlobStorage
    {
        private MemoryStore<string> _memoryStore;

        public InMemoryBlobStorage(MemoryStore<string> memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public Task<Result<Stream>> DownloadAsStreamAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            var result = _memoryStore.Get(container, path)
                .Map(r =>
                {
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(r.Contents));
                    stream.Position = 0;
                    return (Stream)stream;
                });

            return Task.FromResult(result);
        }

        public Task<Result<string>> DownloadAsStringAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            var result = _memoryStore.Get(container, path)
                .Map(r => r.Contents);

            return Task.FromResult(result);
        }

        public Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default)
        {
            var results = _memoryStore.GetAll(container)
                .Map(r => r
                    .Where(i => i.Key.StartsWith(basePath))
                    .Select(i => i.Contents)
                    .ToList());

            return Task.FromResult(results);
        }

        public Task<Result<Done>> UploadAsync(string container, string path, string fileContents, CancellationToken cancellationToken = default)
        {
            _memoryStore.Set(container, path, fileContents);

            return Task.FromResult(Result.Success(Result.Done));
        }

        public Task<Result<Done>> Clear()
        {
            _memoryStore.Clear();

            return Task.FromResult(Result.Success(Result.Done));
        }
    }
}
