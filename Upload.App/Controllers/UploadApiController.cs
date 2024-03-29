﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Upload.App.Models;
using Upload.App.Utils;
using Upload.Domain.Arguments.File;

namespace Upload.App.Controllers
{
    [Authorize]
    [RoutePrefix("api/File")]
    public class UploadApiController : ApiController
    {
        private readonly string root = System.Web.Hosting.HostingEnvironment.MapPath("~/upload");

        public UploadApiController()
        {
        }

        [Route("Upload"), HttpOptions]
        public object UploadFileOptions()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("Upload"), HttpGet]
        public object Upload(int resumableChunkNumber, string resumableIdentifier)
        {
            return ChunkIsHere(resumableChunkNumber, resumableIdentifier) ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [Route("Upload"), HttpPost]
        public async Task<object> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            if (await ReadPart(provider))
            {
                // Success
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                // Fail
                var message = DeleteInvalidChunkData(provider) ? "Cannot read multi part file data." : "Cannot delete temporary file chunk data.";
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new System.Exception(message));
            }
        }

        private static bool DeleteInvalidChunkData(MultipartFormDataStreamProvider provider)
        {
            try
            {
                var localFileName = provider.FileData[0].LocalFileName;
                if (File.Exists(localFileName))
                {
                    File.Delete(localFileName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> ReadPart(MultipartFormDataStreamProvider provider)
        {
            try
            {
                return await Request.Content.ReadAsMultipartAsync(provider).ContinueWith<bool>(x =>
                {
                    if (x.IsFaulted || x.IsCanceled)
                        return false;

                    ResumableConfiguration configuration = GetUploadConfiguration(x.Result);
                    int chunkNumber = GetChunkNumber(x.Result);

                    // Rename generated file
                    MultipartFileData chunk = x.Result.FileData[0]; // Only one file in multipart message
                    RenameChunk(chunk, chunkNumber, configuration.Identifier);

                    // Assemble chunks into single file if they're all here
                    TryAssembleFile(configuration);
                    return true;
                });
            }
            catch
            {
                return false;
            }
        }

        #region Get configuration

        [NonAction]
        private ResumableConfiguration GetUploadConfiguration(MultipartFormDataStreamProvider provider)
        {
            return ResumableConfiguration.Create(identifier: GetId(provider),
                filename: GetFileName(provider),
                chunks: GetTotalChunks(provider),
                size: GetTotalSize(provider));
        }

        [NonAction]
        private string GetFileName(MultipartFormDataStreamProvider provider)
        {
            var filename = provider.FormData["resumableFilename"];
            return !String.IsNullOrEmpty(filename) ? filename : provider.FileData[0].Headers.ContentDisposition.FileName.Trim('\"');
        }

        [NonAction]
        private string GetId(MultipartFormDataStreamProvider provider)
        {
            var id = provider.FormData["resumableIdentifier"];
            return !String.IsNullOrEmpty(id) ? id : Guid.NewGuid().ToString();
        }

        [NonAction]
        private decimal GetTotalSize(MultipartFormDataStreamProvider provider)
        {
            var total = provider.FormData["resumableTotalSize"];
            return !String.IsNullOrEmpty(total) ? Convert.ToDecimal(total) : 0;
        }

        [NonAction]
        private int GetTotalChunks(MultipartFormDataStreamProvider provider)
        {
            var total = provider.FormData["resumableTotalChunks"];
            return !String.IsNullOrEmpty(total) ? Convert.ToInt32(total) : 1;
        }

        [NonAction]
        private int GetChunkNumber(MultipartFormDataStreamProvider provider)
        {
            var chunk = provider.FormData["resumableChunkNumber"];
            return !String.IsNullOrEmpty(chunk) ? Convert.ToInt32(chunk) : 1;
        }

        #endregion

        #region Chunk methods

        [NonAction]
        private string GetChunkFileName(int chunkNumber, string identifier)
        {
            return Path.Combine(root, string.Format("{0}_{1}", identifier, chunkNumber.ToString()));
        }

        [NonAction]
        private void RenameChunk(MultipartFileData chunk, int chunkNumber, string identifier)
        {
            string generatedFileName = chunk.LocalFileName;
            string chunkFileName = GetChunkFileName(chunkNumber, identifier);
            if (File.Exists(chunkFileName)) File.Delete(chunkFileName);
            File.Move(generatedFileName, chunkFileName);

        }

        [NonAction]
        private string GetFilePath(ResumableConfiguration configuration)
        {
            return Path.Combine(root, configuration.Identifier);
        }

        [NonAction]
        private bool ChunkIsHere(int chunkNumber, string identifier)
        {
            string fileName = GetChunkFileName(chunkNumber, identifier);
            return File.Exists(fileName);
        }

        [NonAction]
        private bool AllChunksAreHere(ResumableConfiguration configuration)
        {
            for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
                if (!ChunkIsHere(chunkNumber, configuration.Identifier)) return false;
            return true;
        }

        [NonAction]
        private void TryAssembleFile(ResumableConfiguration configuration)
        {
            if (AllChunksAreHere(configuration))
            {
                // Create a single file
                var path = ConsolidateFile(configuration);
                var extension = configuration.FileName.Split('.');
                var fileName = Guid.NewGuid();
                // Rename consolidated with original name of upload
                RenameFile(path, Path.Combine(root, fileName.ToString() + (extension.Length > 1 ? "." + extension[extension.Length - 1] : "")));

                // Delete chunk files
                DeleteChunks(configuration);

                var request = new InsertFileRequest()
                {
                    CreatedDate = DateTime.Now,
                    IdUser = int.Parse(User.GetUserId()),
                    Name = configuration.FileName,
                    Size = configuration.Size,
                    NomeInterno = fileName
                };

                JsonConvert.DeserializeObject<InsertFileResponse>
                     (ServiceApiUtil.ApiResponse("api/file/Insert", "POST", request));
            }
        }

        [NonAction]
        private void DeleteChunks(ResumableConfiguration configuration)
        {
            for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
            {
                var chunkFileName = GetChunkFileName(chunkNumber, configuration.Identifier);
                File.Delete(chunkFileName);
            }
        }

        [NonAction]
        private string ConsolidateFile(ResumableConfiguration configuration)
        {
            var path = GetFilePath(configuration);
            if (!IsFileLocked(path))
            {
                using (var destStream = File.Create(path, 15000))
                {
                    for (int chunkNumber = 1; chunkNumber <= configuration.Chunks; chunkNumber++)
                    {
                        var chunkFileName = GetChunkFileName(chunkNumber, configuration.Identifier);
                        using (var sourceStream = File.OpenRead(chunkFileName))
                        {
                            sourceStream.CopyTo(destStream);
                        }
                    }
                    destStream.Close();
                }
            }

            return path;
        }

        private bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                if (File.Exists(path))
                {
                    stream = File.Open(path, FileMode.Open);
                }
                else
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        #endregion

        [NonAction]
        private string RenameFile(string sourceName, string targetName)
        {
            targetName = Path.GetFileName(targetName); // Strip to filename if directory is specified (avoid cross-directory attack)
            string realFileName = Path.Combine(root, targetName);
            if (File.Exists(realFileName)) File.Delete(realFileName);
            File.Move(sourceName, realFileName);
            return targetName;
        }
    }
}