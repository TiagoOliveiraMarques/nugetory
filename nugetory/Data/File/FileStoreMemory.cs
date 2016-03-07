using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using nugetory.Exceptions;

namespace nugetory.Data.File
{
    public class FileStoreMemory : IFileStore
    {
        public IDictionary<string, byte[]> PackagesCollection { get; set; }

        public FileStoreMemory()
        {
            PackagesCollection = new Dictionary<string, byte[]>();
        }

        public string SaveFile(Stream filestream, string id)
        {
            string checksum;
            string packageFilename = GetPackageFileLocation(id);

            if (PackagesCollection.ContainsKey(packageFilename))
                throw new AlreadyExistsException();

            byte[] buffer = new byte[16*1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = filestream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                PackagesCollection.Add(packageFilename, ms.ToArray());
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

            if (!PackagesCollection.ContainsKey(packageFilename))
                return null;

            byte[] packageBytes = PackagesCollection[packageFilename];

            using (MD5 md5 = MD5.Create())
            {
                MemoryStream ms = new MemoryStream();

                ms.Write(packageBytes, 0, packageBytes.Length);
                ms.Position = 0;

                if (Convert.ToBase64String(md5.ComputeHash(ms)) != checksum)
                    return null;

                return ms;
            }
        }

        public bool DeleteFile(string id)
        {
            string packageFilename = GetPackageFileLocation(id);

            if (!PackagesCollection.ContainsKey(packageFilename))
                return false;

            PackagesCollection.Remove(packageFilename);

            return true;
        }

        private static string GetPackageFileLocation(string filename)
        {
            return GetPackageFileName(filename);
        }

        public static string GetPackageFileName(string filename)
        {
            return filename + ".nupkg";
        }
    }
}
