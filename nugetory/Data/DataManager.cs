using nugetory.Configuration;
using nugetory.Controllers.Helpers;
using nugetory.Data.DAO;
using nugetory.Data.DB;
using nugetory.Data.Entities;
using nugetory.Data.File;

namespace nugetory.Data
{
    internal class DataManager
    {
        public DataManager(Store configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        private Store ConfigurationStore { get; set; }
        public PackageDAO PackageDAO { get; set; }
        public ICollection<Package> PackageCollection { get; set; }
        public IFileStore FileStore { get; set; }

        public void Start()
        {
            //TODO: allow different types of stores (e.g.: mongodb)
            FileStore = new FileStoreFilesystem(ConfigurationStore);
            PackageCollection = new JSONStore<Package>(ConfigurationStore.DatabaseFile.Value);
            PackageDAO = new PackageDAO(PackageCollection, FileStore);
        }

        public void Stop()
        {
            // nothing to do
        }
    }
}
