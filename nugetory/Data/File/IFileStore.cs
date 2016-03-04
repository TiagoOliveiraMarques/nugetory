using System.IO;

namespace nugetory.Data.File
{
    public interface IFileStore
    {
        string SaveFile(Stream originalFilename, string id);
        Stream GetFile(string id, string checksum);
        bool DeleteFile(string id);
    }
}
