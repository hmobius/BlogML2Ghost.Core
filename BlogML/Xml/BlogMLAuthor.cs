using System;
using System.Xml.Serialization;

namespace BlogML2Ghost.Core.BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLAuthor : BlogMLNode
    {
        [XmlAttribute("email")]
        public string email { get; set; }
    }
}
