using System;
using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable InconsistentNaming

namespace nugetory.Tools
{
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public class feed
    {
        public string id { get; set; }
        public feedTitle title { get; set; }
        public DateTime updated { get; set; }
        public feedLink link { get; set; }

        [XmlElement("entry")]
        public entry[] entry { get; set; }

        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string @base { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedTitle
    {
        public feedTitle()
        {
        }

        public feedTitle(string title)
        {
            type = "text";
            Value = title;
        }

        [XmlAttribute]
        public string type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class feedLink
    {
        public feedLink()
        {
        }

        public feedLink(string title, string href)
        {
            rel = "self";
            this.title = title;
            this.href = href;
        }

        [XmlAttribute]
        public string rel { get; set; }

        [XmlAttribute]
        public string title { get; set; }

        [XmlAttribute]
        public string href { get; set; }
    }
}
