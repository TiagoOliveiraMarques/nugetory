using nugetory.Configuration;
using nugetory.Data.DAO;
using nugetory.Data.DB;
using nugetory.Data.Entities;
using nugetory.Data.File;

namespace nugetory.Data
{
    public class DataManager
    {
        internal DataManager(Store configurationStore)
        {
            ConfigurationStore = configurationStore;
        }

        public static bool DataInMemory { get; set; }
        private Store ConfigurationStore { get; set; }
        public PackageDAO PackageDAO { get; set; }
        internal ICollection<Package> PackageCollection { get; set; }
        internal IFileStore FileStore { get; set; }

        internal void Start()
        {
            if (DataInMemory)
            {
                FileStore = new FileStoreMemory();
                PackageCollection = new JSONMemoryStore<Package>();
            }
            else
            {
                //TODO: allow different types of stores (e.g.: mongodb)
                FileStore = new FileStoreFilesystem(ConfigurationStore);
                PackageCollection = new JSONStore<Package>(ConfigurationStore.DatabaseFile.Value);
            }
            PackageDAO = new PackageDAO(PackageCollection, FileStore);
        }

        internal void Stop()
        {
            // nothing to do
        }
    }
}
