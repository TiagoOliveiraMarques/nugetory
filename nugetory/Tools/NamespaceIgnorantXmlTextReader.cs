using System.IO;
using System.Xml;

namespace nugetory.Tools
{
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(TextReader reader) : base(reader)
        {
        }

        public override string NamespaceURI
        {
            get
            {
                return "";
            }
        }
    }
}
