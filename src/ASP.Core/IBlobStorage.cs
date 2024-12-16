using ASP.Core.Results;

namespace ASP.Core
{
    public interface IBlobStorage
    {
        /// <summary>
        /// Lists the blobs in a specified container and path.
        /// </summary>
        /// <param name="container">The name of the container.</param>
        /// <param name="basePath">The prefix path to filter blobs.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A list of blob names, or an error if the operation fails.</returns>
        Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a blob as binary data.
        /// </summary>
        /// <param name="container">The name of the container.</param>
        /// <param name="path">The blob path within the container.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The downloaded binary data, or an error if the blob is not found or an exception occurs.</returns>
        Task<Result<BinaryData>> DownloadAsync(string container, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a blob to the specified stream.
        /// </summary>
        /// <param name="stream">The target stream for the downloaded content.</param>
        /// <param name="container">The name of the container.</param>
        /// <param name="path">The blob path within the container.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A result indicating success, or an error if the blob is not found or an exception occurs.</returns>
        Task<Result<Done>> DownloadToAsync(Stream stream, string container, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a blob as a stream.
        /// </summary>
        /// <param name="container">The name of the container.</param>
        /// <param name="path">The blob path within the container.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A result containing the blob stream or an error if the blob is not found or an exception occurs.</returns>
        Result<Stream> DownloadStream(string container, string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads content to a blob.
        /// </summary>
        /// <param name="container">The name of the container.</param>
        /// <param name="path">The blob path within the container.</param>
        /// <param name="fileContents">The binary data to upload.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A result indicating success, or an error if the operation fails.</returns>
        Task<Result<Done>> UploadAsync(string container, string path, BinaryData fileContents, CancellationToken cancellationToken = default);

        /// <summary>
        /// Clears all blobs in the container. Not implemented
        /// </summary>
        /// <returns>NotImplementedException.</returns>
        Task<Result<Done>> ClearAsync();
    }
}
