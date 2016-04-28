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
        
        public static PackageDependency[] Process(packageMetadataDependencies dependencies)
        {
            if (dependencies == null)
                return null;
            
            // This is non standard behaviour. The nuspec should NOT have both grouped and ungrouped dependencies.
            // In case of both existing, just ignore the ungrouped ones.
            if (dependencies.group != null)
                return dependencies.group.SelectMany(g => g.dependency).Select(d => new PackageDependency(d.id, d.version)).ToArray();

            return dependencies.dependency.Select(d => new PackageDependency(d.id, d.version)).ToArray();
        }
    }
}
