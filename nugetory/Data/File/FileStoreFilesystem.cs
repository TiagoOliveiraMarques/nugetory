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

        public string SaveFile(string originalFilename, string id)
        {
            string checksum;
            string packageFilename = GetPackageFileLocation(id);

            if (System.IO.File.Exists(packageFilename))
                throw new AlreadyExistsException();

            // allow exception to be thrown in case of error
            System.IO.File.Move(originalFilename, packageFilename);

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

            using (MD5 md5 = MD5.Create())
            using (FileStream stream = System.IO.File.OpenRead(packageFilename))
            {
                if (Convert.ToBase64String(md5.ComputeHash(stream)) != checksum)
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
            return filename + ".nupkg";
        }
    }
}
