// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can be stored about a tag in ghost.json
using Newtonsoft.Json;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostTag
    {
        public int id { get; set; }  
        public string name { get; set; }  // "my tag name"
        public string slug { get; set; }  //  "my-tag-name" - max allowed limit of 191 chars
        public string description { get; set; }  //  "my tag description"

        [JsonIgnore]
        public string BlogMLid { get; set; } // id from blogml file.
    }
}
