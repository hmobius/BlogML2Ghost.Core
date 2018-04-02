// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can be stored about a user in ghost.json

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostUser
    {
        public int id { get; set; }
        public string name { get; set; }  //  "User's name"
        public string slug { get; set; }  //  "users-name" - max allowed limit of 191 chars
        public string ghost_auth_access_token { get; set; } // null
        public string ghost_auth_id { get; set; } // null
        public string password { get; set; }
        public string email { get; set; }  // "user@example.com"
        public string profile_image { get; set; } //  null - could be gravatar image for instance
        public string cover_image { get; set; } //  null
        public string bio { get; set; }  //  null
        public string website { get; set; }  //  null
        public string location { get; set; } // null
        public string facebook { get; set; } // null
        public string twitter { get; set; } // null
        public string accessibility { get; set; } //  null
        public GhostUserStatus status { get; set; }  //  "active"
        public CultureInfo locale { get; set; }  // "en_US" 
        public GhostVisibility visibility { get; set; } // "public"
        public string meta_title { get; set; }  // null
        public string meta_description { get; set; }  //  null
        public string tour { get; set; } // null
        public DateTime last_seen { get; set; }  // as yyyy-MM-dd hh:mm:ss
        public DateTime created_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int created_by { get; set; }  // the first user created has an id of 1
        public DateTime updated_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int updated_by { get; set; }  // the first user created has an id of 1
    }

    public enum GhostUserStatus
    {
        active = 0,
        disabled
    }
}
