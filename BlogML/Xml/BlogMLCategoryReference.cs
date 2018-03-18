using System;
using System.Xml.Serialization;

namespace BlogML2Ghost.Core.BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLCategoryReference
    {
       [XmlAttribute("ref")]
        public string Ref { get; set; }
    }
}
