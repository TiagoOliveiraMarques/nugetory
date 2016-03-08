using System;
using System.IO;
using System.Security.Cryptography;
using nugetory.Configuration;
using nugetory.Exceptions;

namespace nugetory.Data.File
{
    public class FileStoreFilesystem : IFileStore
    {
        private Store ConfigurationStore { get; set; }
        public string PackagesDirectory { get; set; }

        public FileStoreFilesystem(Store configurationStore)
        {
            ConfigurationStore = configurationStore;
            PackagesDirectory = ConfigurationStore.DatabasePackagesDirectory.Value;

            if (PackagesDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                PackagesDirectory = PackagesDirectory + Path.DirectorySeparatorChar;

            if (!Directory.Exists(PackagesDirectory))
                Directory.CreateDirectory(PackagesDirectory);
        }

        public string SaveFile(Stream uploadStream, string id)
        {
            string checksum;
            string packageFilename = GetPackageFileLocation(id);

            if (System.IO.File.Exists(packageFilename))
                throw new AlreadyExistsException();

            // allow exception to be thrown in case of error
            using (FileStream fs = new FileStream(packageFilename, FileMode.Create, FileAccess.Write))
            {
                uploadStream.CopyTo(fs);
            }

            using (SHA512 sha512 = SHA512.Create())
            using (FileStream stream = System.IO.File.OpenRead(packageFilename))
            {
                checksum = Convert.ToBase64String(sha512.ComputeHash(stream));
            }

            return checksum;
        }

        public Stream GetFile(string id, string checksum)
        {
            string packageFilename = GetPackageFileLocation(id);

            if (!System.IO.File.Exists(packageFilename))
                return null;

            using (SHA512 sha512 = SHA512.Create())
            using (FileStream stream = System.IO.File.OpenRead(packageFilename))
            {
                if (Convert.ToBase64String(sha512.ComputeHash(stream)) != checksum)
                    return null;
            }

            return System.IO.File.OpenRead(packageFilename);
        }

        public bool DeleteFile(string id)
        {
            string packageFilename = GetPackageFileLocation(id);

            if (!System.IO.File.Exists(packageFilename))
                return false;

            try
            {
                System.IO.File.Delete(packageFilename);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private string GetPackageFileLocation(string filename)
        {
            return PackagesDirectory + GetPackageFileName(filename);
        }

        public static string GetPackageFileName(string filename)
        {
            return filename.ToLowerInvariant() + ".nupkg";
        }
    }
}
