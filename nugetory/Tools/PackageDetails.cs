using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable InconsistentNaming

namespace nugetory.Tools
{
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public class entry
    {
        [XmlElement]
        public string id { get; set; }

        [XmlElement]
        public entryCategory category { get; set; }

        [XmlElement]
        public entryLink[] link { get; set; }

        [XmlElement]
        public entryTitle title { get; set; }

        [XmlElement]
        public entrySummary summary { get; set; }

        [XmlElement]
        public string updated { get; set; }

        [XmlElement]
        public entryAuthor author { get; set; }

        [XmlElement]
        public entryContent content { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public properties properties { get; set; }

        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string @base { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", IsNullable = false)]
    public class properties
    {
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Version { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string NormalizedVersion { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Copyright { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Created Created { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Dependencies { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Description { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public DownloadCount DownloadCount { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string GalleryDetailsUrl { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string IconUrl { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public IsLatestVersion IsLatestVersion { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public IsAbsoluteLatestVersion IsAbsoluteLatestVersion { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public IsPrerelease IsPrerelease { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Language { get; set; }
        
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public LastUpdated LastUpdated { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Published Published { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string PackageHash { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string PackageHashAlgorithm { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public PackageSize PackageSize { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string ProjectUrl { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string ReportAbuseUrl { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string ReleaseNotes { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public RequireLicenseAcceptance RequireLicenseAcceptance { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Tags { get; set; }
        
        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Title { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Summary { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Authors { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Id { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public VersionDownloadCount VersionDownloadCount { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public MinClientVersion MinClientVersion { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public LastEdited LastEdited { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string LicenseUrl { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public LicenseNames LicenseNames { get; set; }

        [XmlElement(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public LicenseReportUrl LicenseReportUrl { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class Created
    {
        public Created()
        {
        }

        public Created(string date)
        {
            type = "Edm.DateTime";
            Value = date;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public string Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class DownloadCount
    {
        public DownloadCount()
        {
        }

        public DownloadCount(uint count)
        {
            type = "Edm.Int32";
            Value = count;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public uint Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class IsLatestVersion
    {
        public IsLatestVersion()
        {
        }

        public IsLatestVersion(bool latest)
        {
            type = "Edm.Boolean";
            Value = latest;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public bool Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class IsAbsoluteLatestVersion
    {
        public IsAbsoluteLatestVersion()
        {
        }

        public IsAbsoluteLatestVersion(bool latest)
        {
            type = "Edm.Boolean";
            Value = latest;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public bool Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class IsPrerelease
    {
        public IsPrerelease()
        {
        }

        public IsPrerelease(bool pre)
        {
            type = "Edm.Boolean";
            Value = pre;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public bool Value { get; set; }
    }
    
    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class Published
    {
        public Published()
        {
        }

        public Published(string date)
        {
            type = "Edm.DateTime";
            Value = date;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public string Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class LastUpdated
    {
        public LastUpdated()
        {
        }

        public LastUpdated(string date)
        {
            type = "Edm.DateTime";
            Value = date;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public string Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class PackageSize
    {
        public PackageSize()
        {
        }

        public PackageSize(uint size)
        {
            type = "Edm.Int64";
            Value = size;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public uint Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class RequireLicenseAcceptance
    {
        public RequireLicenseAcceptance()
        {
        }

        public RequireLicenseAcceptance(bool require)
        {
            type = "Edm.Boolean";
            Value = require;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public bool Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class VersionDownloadCount
    {
        public VersionDownloadCount()
        {
        }

        public VersionDownloadCount(ushort count)
        {
            type = "Edm.Int32";
            Value = count;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public ushort Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class MinClientVersion
    {
        public MinClientVersion()
        {
            @null = true;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public bool @null { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class LastEdited
    {
        public LastEdited()
        {
            type = "Edm.DateTime";
            @null = true;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string type { get; set; }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public bool @null { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class LicenseNames
    {
        public LicenseNames()
        {
            @null = true;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public bool @null { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices", IsNullable = false)]
    public class LicenseReportUrl
    {
        public LicenseReportUrl()
        {
            @null = true;
        }

        /// <remarks />
        [XmlAttribute(Form = XmlSchemaForm.Qualified,
            Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public bool @null { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryAuthor
    {
        public entryAuthor()
        {
        }

        public entryAuthor(string name)
        {
            this.name = name;
        }

        /// <remarks />
        public string name { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryCategory
    {
        public entryCategory()
        {
            term = "NuGetGallery.V2FeedPackage";
            scheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";
        }

        public entryCategory(string term)
        {
            this.term = term;
            scheme = "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme";
        }

        /// <remarks />
        [XmlAttribute]
        public string term { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string scheme { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryContent
    {
        public entryContent()
        {
        }

        public entryContent(string src)
        {
            type = "application/zip";
            this.src = src;
        }

        /// <remarks />
        [XmlAttribute]
        public string type { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string src { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryLink
    {
        public entryLink()
        {
        }

        public entryLink(string rel, string href)
        {
            this.rel = rel;
            title = "V2FeedPackage";
            this.href = href;
        }

        /// <remarks />
        [XmlAttribute]
        public string rel { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string title { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string href { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entrySummary
    {
        public entrySummary()
        {
        }

        public entrySummary(string summary)
        {
            type = "text";
            Value = summary;
        }

        /// <remarks />
        [XmlAttribute]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public string Value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryTitle
    {
        public entryTitle()
        {
        }

        public entryTitle(string title)
        {
            type = "text";
            Value = title;
        }

        /// <remarks />
        [XmlAttribute]
        public string type { get; set; }

        /// <remarks />
        [XmlText]
        public string Value { get; set; }
    }
}
