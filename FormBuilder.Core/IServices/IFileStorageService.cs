namespace FormBuilder.Core.IServices
{
    /// <summary>
    /// Interface for file storage operations
    /// Supports local file system and can be extended for cloud storage (Azure Blob, AWS S3, etc.)
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves a file to storage and returns the file path
        /// </summary>
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string? subFolder = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a file stream from storage
        /// </summary>
        Task<Stream?> GetFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file from storage
        /// </summary>
        Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a file exists
        /// </summary>
        Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets file information (size, last modified, etc.)
        /// </summary>
        Task<FileInfo?> GetFileInfoAsync(string filePath, CancellationToken cancellationToken = default);
    }

    public class FileInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string ContentType { get; set; } = string.Empty;
    }
}
