using ASP.Core;
using ASP.Core.Results;

namespace ASP.Infrastructure.InMemory
{
    public class InMemoryBlobStorage : IBlobStorage
    {
        private MemoryStore<string> _memoryStore;

        public InMemoryBlobStorage(MemoryStore<string> memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public async Task<Result<Done>> DownloadToAsync(Stream stream, string container, string path, CancellationToken cancellationToken = default)
        {
            var result = await _memoryStore.Get(container, path)
                .MapError(error => error is NotFoundError ? Error.NotFound($"Blob storage file \"{path}\" does not exist in container \"{container}\".") : error)
                .Map(async r =>
                {
                    using (var writer = new StreamWriter(stream, leaveOpen: true))
                    {
                        await writer.WriteAsync(r.Contents);
                    }

                    return Result.Done;
                });

            return result;
        }

        public Task<Result<BinaryData>> DownloadAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            var result = _memoryStore.Get(container, path)
                                     .MapError(error => error is NotFoundError ? Error.NotFound($"Blob storage file \"{path}\" does not exist in container \"{container}\".") : error)
                                     .Map(r => BinaryData.FromString(r.Contents));

            return Task.FromResult(result);
        }

        public Result<Stream> DownloadStream(string container, string path, CancellationToken cancellationToken = default)
        {
            var result = _memoryStore.Get(container, path)
                .Map(r =>
                {
                    var stream = new MemoryStream();
                    using (var writer = new StreamWriter(stream, leaveOpen: true))
                    {
                        writer.Write(r.Contents);
                    }
                    stream.Position = 0;
                    return (Stream)stream;
                });

            return result;
        }

        public Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default)
        {
            var results = _memoryStore.GetAll(container)
                .Map(r => r
                    .Where(i => i.Key.StartsWith(basePath))
                    .Select(i => i.Key)
                    .ToList());

            return Task.FromResult(results);
        }

        public Task<Result<Done>> UploadAsync(string container, string path, BinaryData fileContents, CancellationToken cancellationToken = default)
        {
            _memoryStore.Set(container, path, fileContents.ToString());

            return Task.FromResult(Result.Success(Result.Done));
        }

        public Task<Result<Done>> ClearAsync()
        {
            _memoryStore.Clear();

            return Task.FromResult(Result.Success(Result.Done));
        }

        public Task<Result<Done>> ClearContainer(string container)
        {
            _memoryStore.ClearContainer(container);

            return Task.FromResult(Result.Success(Result.Done));
        }
    }
}
