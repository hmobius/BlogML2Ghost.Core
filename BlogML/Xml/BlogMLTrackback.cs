using System;
using System.Xml.Serialization;

namespace BlogML2Ghost.Core.BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLTrackback : BlogMLNode
    {
        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}
