namespace Services.Interfaces;

public interface IFileStorageClient
{
    string GetDownloadUrl(string filePath);
    string GetUploadUrl(string filePath);
    void DeleteFile(string filePath);
}
