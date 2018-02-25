using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
                Console.WriteLine("Invalid arguments");
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
            throw new NotImplementedException();
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
