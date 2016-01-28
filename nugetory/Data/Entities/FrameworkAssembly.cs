using System.Linq;
using nugetory.Tools;

namespace nugetory.Data.Entities
{
    public class FrameworkAssembly
    {
        public string AssemblyName { get; set; }
        public string TargetFramework { get; set; }

        public FrameworkAssembly()
        {
        }

        public FrameworkAssembly(string assemblyName, string targetFramework)
        {
            AssemblyName = assemblyName;
            TargetFramework = targetFramework;
        }

        public static FrameworkAssembly[] Process(packageMetadataFrameworkAssembly[] assemblies)
        {
            return assemblies == null
                       ? null
                       : assemblies.ToList()
                                   .Select(d => new FrameworkAssembly(d.assemblyName, d.targetFramework))
                                   .ToArray();
        }
    }
}
