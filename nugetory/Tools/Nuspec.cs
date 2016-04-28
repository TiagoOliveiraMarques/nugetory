using System.Xml.Serialization;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable ArrangeThisQualifier
// ReSharper disable InconsistentNaming

namespace nugetory.Tools
{
    public class package
    {
        private packageMetadata metadataField;

        public packageMetadata metadata
        {
            get
            {
                return this.metadataField;
            }
            set
            {
                this.metadataField = value;
            }
        }
    }

    public class packageMetadata
    {
        private string idField;

        private string versionField;

        private string titleField;

        private string authorsField;

        private string ownersField;

        private string licenseUrlField;

        private string projectUrlField;

        private string iconUrlField;

        private bool requireLicenseAcceptanceField;

        private string descriptionField;

        private string summaryField;

        private string releaseNotesField;

        private string copyrightField;

        private string languageField;

        private string tagsField;
        
        private packageMetadataDependencies dependenciesField;
        
        private packageMetadataFrameworkAssembly[] frameworkAssembliesField;

        private string minClientVersionField;

        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        public string authors
        {
            get
            {
                return this.authorsField;
            }
            set
            {
                this.authorsField = value;
            }
        }

        public string owners
        {
            get
            {
                return this.ownersField;
            }
            set
            {
                this.ownersField = value;
            }
        }

        public string licenseUrl
        {
            get
            {
                return this.licenseUrlField;
            }
            set
            {
                this.licenseUrlField = value;
            }
        }

        public string projectUrl
        {
            get
            {
                return this.projectUrlField;
            }
            set
            {
                this.projectUrlField = value;
            }
        }

        public string iconUrl
        {
            get
            {
                return this.iconUrlField;
            }
            set
            {
                this.iconUrlField = value;
            }
        }

        public bool requireLicenseAcceptance
        {
            get
            {
                return this.requireLicenseAcceptanceField;
            }
            set
            {
                this.requireLicenseAcceptanceField = value;
            }
        }

        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        public string summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        public string releaseNotes
        {
            get
            {
                return this.releaseNotesField;
            }
            set
            {
                this.releaseNotesField = value;
            }
        }

        public string copyright
        {
            get
            {
                return this.copyrightField;
            }
            set
            {
                this.copyrightField = value;
            }
        }

        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        public string tags
        {
            get
            {
                return this.tagsField;
            }
            set
            {
                this.tagsField = value;
            }
        }
        
        public packageMetadataDependencies dependencies
        {
            get
            {
                return this.dependenciesField;
            } set
            {
                this.dependenciesField = value;
            }
        }

        [XmlArrayItem("frameworkAssembly", IsNullable = false)]
        public packageMetadataFrameworkAssembly[] frameworkAssemblies
        {
            get
            {
                return this.frameworkAssembliesField;
            }
            set
            {
                this.frameworkAssembliesField = value;
            }
        }

        [XmlAttribute]
        public string minClientVersion
        {
            get
            {
                return this.minClientVersionField;
            }
            set
            {
                this.minClientVersionField = value;
            }
        }
    }

    public class packageMetadataDependencies
    {
        [XmlElement]
        public packageMetadataDependency[] dependency { get; set; }

        [XmlElement]
        public packageMetadataDependencyGroup[] group { get; set; }
    }

    public class packageMetadataDependencyGroup
    {
        [XmlAttribute]
        public string targetFramework { get; set; }


        [XmlElement]
        public packageMetadataDependency[] dependency { get; set; }
    }

    public class packageMetadataDependency
    {
        private string idField;

        private string versionField;

        [XmlAttribute]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [XmlAttribute]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    public class packageMetadataFrameworkAssembly
    {
        private string assemblyNameField;

        private string targetFrameworkField;

        [XmlAttribute]
        public string assemblyName
        {
            get
            {
                return this.assemblyNameField;
            }
            set
            {
                this.assemblyNameField = value;
            }
        }

        [XmlAttribute]
        public string targetFramework
        {
            get
            {
                return this.targetFrameworkField;
            }
            set
            {
                this.targetFrameworkField = value;
            }
        }
    }
}
