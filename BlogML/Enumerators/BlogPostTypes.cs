using System;
using System.Xml.Serialization;

namespace BlogML2Ghost.Core.BlogML
{
    public enum BlogPostTypes : short
    {
        [System.Xml.Serialization.XmlEnumAttribute("normal")]
        Normal = 1,
        [System.Xml.Serialization.XmlEnumAttribute("article")]
        Article = 2,
    }
}
