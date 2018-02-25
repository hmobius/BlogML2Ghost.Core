using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BlogML2Ghost.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO Nicer command line here - pull proper command line parser?
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: blogml2ghost <input.xml> <output.json>");
                return;
            }

            string inputFileName= args[0];
            string outputFileName = args[1];

            // TODO Pull out ghost importformat into external classes
            const string DataVersion = "003";

            var metaData = new {
                exported_on = GetEpochTime(DateTime.UtcNow),
                version = DataVersion
            };

            // TODO Make conversion process more explicit
            var convertedData = ConvertInputFile(inputFileName);

            // TODO Split serailization into function
            var rootData = new {
                meta = metaData,
                data = convertedData
            };

            string jsonData = JsonConvert.SerializeObject(rootData, 
                new JsonSerializerSettings{
                    NullValueHandling = NullValueHandling.Include
                });

            File.WriteAllText(outputFileName, jsonData);

            Console.WriteLine("Success!");
        }

        private static object ConvertInputFile(string input)
        {
            XNamespace xNamespace = "http://www.blogml.com/2006/09/BlogML";            

            var root = XElement.Load(File.OpenRead(input));            
            var postRoot = root.Element(xNamespace + "posts");
            var posts = postRoot.Elements(xNamespace + "post");
            var convertedPosts = new List<object>();
            var tagsBelongingToPost = new List<object>();

            var categories = root.Element(xNamespace + "categories")
                .Elements(xNamespace + "category")
                .Select((p, s) => new {p, s })
                .ToDictionary(t => (Guid)t.p.Attribute("id"), v => new
                {
                    id = v.s,
                    name = v.p.Element(xNamespace + "title").Value,
                    slug = GetSlugFromTitle(v.p.Element(xNamespace + "title").Value),
                    description = ""
                });

            int postId = 0;            
            foreach (var post in posts)
            {
                postId++;
                convertedPosts.Add(new
                {
                    id = postId,
                    uuid = Guid.NewGuid(),
                    title = post.Element(xNamespace + "title").Value,
                    slug = GetSlugFromTitle(post.Element(xNamespace + "title").Value),
                    markdown = post.Element(xNamespace + "content").Value,
                    html = post.Element(xNamespace + "content").Value,
                    image = (object)null,
                    featured = 0,
                    page = 0,
                    status = "published",
                    language = "en_GB",
                    meta_title = (object)null,
                    meta_description = (object)null,
                    author_id = 1,
                    created_at = GetEpochTime((DateTime)post.Attribute("date-created")),
                    created_by = 1,
                    updated_at = GetEpochTime((DateTime)post.Attribute("date-modified")),
                    updated_by = 1,
                    published_at = GetEpochTime((DateTime)post.Attribute("date-created")),
                    published_by = 1
                });

                var categoryTags = post.Element(xNamespace + "categories");
                if (categoryTags != null)
                {
                    var id = postId;
                    var tags = categoryTags
                        .Elements(xNamespace + "category")
                        .Select(x => new { post_id = id, tag_id = categories[(Guid)x.Attribute("ref")].id });

                    tagsBelongingToPost.AddRange(tags);
                }
            }

            return new
            {
                posts = convertedPosts,
                tags = categories.Values.ToList(),
                posts_tags = tagsBelongingToPost
            };
        }


        // TODO Rewrite as DateTime extenstion AsMillisecondsSinceEpoch
        private static long GetEpochTime(DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime - epoch).TotalMilliseconds;
        }

        // TODO Rewrite within BlogML class?
        private static string GetSlugFromTitle(string url)
        {
            return Regex.Replace(Regex.Replace(RemoveDiacritics(url), "[^A-Za-z0-9-]+", "-"), "-{2,}", "-");
        }
        
        public static string RemoveDiacritics(string text)
        {
            Encoding srcEncoding = Encoding.UTF8;
            Encoding destEncoding = Encoding.GetEncoding(1252); // Latin alphabet

            text = destEncoding.GetString(Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(text)));

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                if (!CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]).Equals(UnicodeCategory.NonSpacingMark))
                {
                    builder.Append(normalizedString[i]);
                }
            }

            return builder.ToString();
        }        
    }
}
