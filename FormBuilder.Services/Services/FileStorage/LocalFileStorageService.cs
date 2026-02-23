using FormBuilder.Core.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using FileInfo = FormBuilder.Core.IServices.FileInfo;

namespace FormBuilder.Services.Services.FileStorage
{
    /// <summary>
    /// Local file system storage implementation
    /// </summary>
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly ILogger<LocalFileStorageService> _logger;

        public LocalFileStorageService(IConfiguration configuration, ILogger<LocalFileStorageService> logger)
        {
            _logger = logger;
            _basePath = configuration["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            
            // Ensure base directory exists
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
                _logger.LogInformation("Created file storage directory: {BasePath}", _basePath);
            }
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string? subFolder = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // Sanitize file name
                var sanitizedFileName = SanitizeFileName(fileName);
                
                // Create subfolder path if specified
                var targetDirectory = string.IsNullOrEmpty(subFolder) 
                    ? _basePath 
                    : Path.Combine(_basePath, subFolder);

                // Ensure directory exists
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Generate unique file name to avoid conflicts
                var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
                var filePath = Path.Combine(targetDirectory, uniqueFileName);

                // Save file
                using (var fileStreamOut = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await fileStream.CopyToAsync(fileStreamOut, cancellationToken);
                }

                // Return relative path from base directory
                var relativePath = Path.GetRelativePath(_basePath, filePath).Replace('\\', '/');
                _logger.LogInformation("File saved successfully: {FilePath}", relativePath);

                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file: {FileName}", fileName);
                throw;
            }
        }

        public async Task<Stream?> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(_basePath, filePath);

                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning("File not found: {FilePath}", fullPath);
                    return null;
                }

                // Return a new stream for the file
                return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading file: {FilePath}", filePath);
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(_basePath, filePath);

                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", fullPath);
                    return false;
                }

                File.Delete(fullPath);
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(_basePath, filePath);
                return await Task.FromResult(File.Exists(fullPath));
            }
            catch
            {
                return false;
            }
        }

        public async Task<FileInfo?> GetFileInfoAsync(string filePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var fullPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(_basePath, filePath);

                if (!File.Exists(fullPath))
                {
                    return null;
                }

                var fileInfo = new System.IO.FileInfo(fullPath);
                var contentType = GetContentType(filePath);

                return await Task.FromResult(new FileInfo
                {
                    FilePath = filePath,
                    Size = fileInfo.Length,
                    LastModified = fileInfo.LastWriteTimeUtc,
                    ContentType = contentType
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file info: {FilePath}", filePath);
                return null;
            }
        }

        private string SanitizeFileName(string fileName)
        {
            // Remove invalid characters
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            
            // Limit length
            if (sanitized.Length > 255)
            {
                var extension = Path.GetExtension(sanitized);
                sanitized = sanitized.Substring(0, 255 - extension.Length) + extension;
            }

            return sanitized;
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                ".json" => "application/json",
                ".xml" => "application/xml",
                _ => "application/octet-stream"
            };
        }
    }
}
