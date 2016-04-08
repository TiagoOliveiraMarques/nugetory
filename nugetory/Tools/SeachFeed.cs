using System;

namespace nugetory.Tools
{
    public class SeachFeed
    {
        /// <remarks />
        [Serializable]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
        public class feed
        {
            public feed()
            {
                id = "http://schemas.datacontract.org/2004/07/";
            }

            /// <remarks />
            [System.Xml.Serialization.XmlElementAttribute(
                Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
            public int count { get; set; }

            /// <remarks />
            public string id { get; set; }

            /// <remarks />
            public object title { get; set; }

            /// <remarks />
            public DateTime updated { get; set; }

            /// <remarks />
            public feedLink link { get; set; }

            /// <remarks />
            [System.Xml.Serialization.XmlElementAttribute("entry")]
            public entry[] entry { get; set; }

            /// <remarks />
            [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified,
                Namespace = "http://www.w3.org/XML/1998/namespace")]
            public string @base { get; set; }
        }

        /// <remarks />
        [Serializable]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedLink
        {
            public feedLink()
            {
                
            }

            public feedLink(string href)
            {
                rel = "self";
                this.href = href;
            }

            /// <remarks />
            [System.Xml.Serialization.XmlAttributeAttribute]
            public string rel { get; set; }

            /// <remarks />
            [System.Xml.Serialization.XmlAttributeAttribute]
            public string href { get; set; }
        }
    }
}
