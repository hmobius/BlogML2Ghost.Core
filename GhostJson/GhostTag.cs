// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can be stored about a tag in ghost.json
using System;
using Newtonsoft.Json;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostTag
    {
        public int id { get; set; }  // start at 1 if importing into fresh installation
        public string name { get; set; }  // "my tag name"
        public string slug { get; set; }  //  "my-tag-name" - max allowed limit of 191 chars
        public string description { get; set; }  //  "my tag description"
        public string feature_image { get; set; } // null
        public int? parent_id { get; set; } // set to parent tag id or null
        public GhostVisibility visibility { get; set; }
        public string meta_title { get; set; } // null
        public string meta_description { get; set; } // null
        public DateTime created_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int created_by { get; set; }  // the first user created has an id of 1
        public DateTime updated_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int updated_by { get; set; }  // the first user created has an id of 1 

        [JsonIgnore]
        public string BlogMLid { get; set; } // id from blogml file.
    }
}
