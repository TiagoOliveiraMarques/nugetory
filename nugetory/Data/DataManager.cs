using nugetory.Controllers;
using nugetory.Controllers.Helpers;
using nugetory.Data.DAO;
using nugetory.Data.DB;
using nugetory.Data.Entities;
using nugetory.Data.File;

namespace nugetory.Data
{
    internal class DataManager
    {
        public PackageDAO PackageDAO { get; set; }
        public ICollection<Package> PackageCollection { get; set; }
        public IFileStore FileStore { get; set; }

        public void Start()
        {
            //TODO: allow different types of stores (e.g.: mongodb)
            FileStore = new FileStoreFilesystem(Configuration.PackagesDirectory);
            PackageCollection = new JSONStore<Package>(Configuration.ConfigurationFile);
            PackageDAO = new PackageDAO(PackageCollection, FileStore);

            // inject
            UploadPackage.PackageDAO = PackageDAO;
            DownloadPackage.PackageDAO = PackageDAO;
            DownloadPackage.FileStore = FileStore;
            PackageDetails.PackageDAO = PackageDAO;
            PackageDetails.FileStore = FileStore;
            DeletePackage.PackageDAO = PackageDAO;
            DeletePackage.FileStore = FileStore;
            FindPackage.PackageDAO = PackageDAO;
        }

        public void Stop()
        {
            // nothing to do
        }
    }
}
