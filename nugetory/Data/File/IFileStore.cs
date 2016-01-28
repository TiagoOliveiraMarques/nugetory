using System.IO;

namespace nugetory.Data.File
{
    public interface IFileStore
    {
        string SaveFile(string originalFilename, string id);
        Stream GetFile(string id, string checksum);
        bool DeleteFile(string id);
    }
}
