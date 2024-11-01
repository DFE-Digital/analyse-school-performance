using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Time;
using ASP.Web.Core.Templating;
using ASP.Web.Features.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using ASP.Web.Extensions;

namespace ASP.Test.Web.Areas.BlobStorageTest
{
    [Area("BlobStorageDemo")]
    [Route("blob-storage-demo")]
    [Authorize(Policy.AdminOnly)]
    public class BlobStorageDemoController : Controller
    {
        public const string TEMPLATE_ID = "blob-storage-demo";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IBlobStorage _blobStorage;
        private readonly CurrentTimeProvider _currentTimeProvider;

        public BlobStorageDemoController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IBlobStorage blobStorage,
            CurrentTimeProvider currentTimeProvider
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _blobStorage = blobStorage;
            _currentTimeProvider = currentTimeProvider;
        }

        [HttpGet("")]
        public new async Task<IActionResult> View()
        {
            return await _api.ViewContentTemplate(new ViewContentTemplateRequest(TEMPLATE_ID, Optional.FromNullable(TEMPLATE_ID)))
                .Map(t => ContentTemplateViewModel.FromTemplate(TEMPLATE_ID, TEMPLATE_ID, t))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("file-download")]
        public IActionResult FileDownload(string container, string filepath)
        {
            return (
                from stream in _blobStorage.DownloadStream(container, filepath)
                select new FileStreamResult(stream, "text/csv") {
                    FileDownloadName = Path.GetFileName(filepath)
                }
            ).ToActionResult(_hostEnvironment);
        }

        [HttpGet("api-file-download")]
        public async Task<IActionResult> ApiFileDownload(string container, string filepath)
        {
            var result = await _api.BlobStorageDemoFileDownload(new(container, filepath));

            return result.ToActionResult(response => new FileStreamResult(response.Content, response.ContentType) {
                FileDownloadName = response.FileName
            }, _hostEnvironment);
        }

        [HttpGet("zip-file-download")]
        public async Task<IActionResult> ZipDownload(string container, string filepath)
        {
            var filename = $"{_currentTimeProvider.CurrentTime:yyyyMMdd_HHmmss}_asp_blob_storage_demo.zip";
            Response.ContentType = "application/zip";
            Response.Headers.Append("Content-Disposition", $"attachment; filename={filename}; filename*=UTF-8''{filename}");

            using (var archive = new ZipArchive(Response.BodyWriter.AsStream(), ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(filepath, CompressionLevel.Fastest);
                    
                using var entryStream = entry.Open();
                await _blobStorage.DownloadToAsync(entryStream, container, filepath);
            }

            return new OkResult();
        }

        [HttpGet("api-zip-file-download")]
        public async Task<IActionResult> ApiZipFileDownload(string container, string filepath)
        {
            var result = await _api.BlobStorageDemoZipFileDownload(new(container, filepath));

            return result.ToActionResult(response => new FileStreamResult(response.Content, response.ContentType) {
                FileDownloadName = response.FileName
            }, _hostEnvironment);
        }

        [HttpGet("api-download-as-zip-file")]
        public async Task<IActionResult> ApiDownloadAsZipFile(List<string> downloadIds)
        {
            var result = await _api.DownloadAsZipFile(new(Core.Utilities.FileType.CSV, downloadIds));
            
            return result.ToActionResult(response => new FileStreamResult(response.Content, response.ContentType) {
                FileDownloadName = response.FileName
            }, _hostEnvironment);
        }
    }
}