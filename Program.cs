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
            BlogML2GhostConverter converter = new BlogML2GhostConverter();
            converter.Run();
             Console.WriteLine("Success!");
        }
    }
}
