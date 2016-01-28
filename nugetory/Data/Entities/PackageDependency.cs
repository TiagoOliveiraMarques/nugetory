using System.Linq;
using nugetory.Tools;

namespace nugetory.Data.Entities
{
    public class PackageDependency
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public PackageDependency()
        {
        }

        public PackageDependency(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public static PackageDependency[] Process(packageMetadataDependency[] dependencies)
        {
            return dependencies == null
                       ? null
                       : dependencies.ToList().Select(d => new PackageDependency(d.id, d.version)).ToArray();
        }
    }
}
