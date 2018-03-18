
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace BlogML2Ghost.Core.BlogML
{
    public class BlogMLValidator
    {
        public bool IsBlogMLFileValid(string inputFileUrl)
        {
            XDocument blog = XDocument.Load(inputFileUrl);
            StringBuilder log = new StringBuilder();
            bool valid = true;
            blog.Validate(
                GetBlogMLSchemaSet(),
                (o,e) => {
                    log.AppendFormat("Validation {0} : {1}", e.Severity, e.Message).AppendLine();
                    valid = false;
                }
            );
            if (log.Length > 0)
            {
                Console.WriteLine("File is not valid BlogML");
                Console.Write(log.ToString());
            }
            return valid;
        }

        public XmlSchemaSet GetBlogMLSchemaSet()
        {
            using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BlogML2Ghost.Core.lib.blogml.xsd"))
            {
                if (resourceStream == null)
                {
                    throw new InvalidOperationException("Schema resource not found");
                }

                using(XmlReader xmlResource = XmlReader.Create(resourceStream))
                {
                    XmlSchemaSet schemaSet = new XmlSchemaSet();
                    schemaSet.Add(BlogMLNamespace.Value, xmlResource);
                    return schemaSet;
                }
            }
        }
    }
}


