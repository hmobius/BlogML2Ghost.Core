// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can be stored about a user in ghost.json

using System.Globalization;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostUser
    {
        public int id { get; set; }
        public string name { get; set; }  //  "User's name"
        public string slug { get; set; }  //  "users-name" - max allowed limit of 191 chars
        public string email { get; set; }  // "user@example.com"
        public string image { get; set; } //  null
        public string cover { get; set; } //  null
        public string bio { get; set; }  //  null
        public string website { get; set; }  //  null
        public string location { get; set; } // null
        public string accessibility { get; set; } //  null
        public GhostUserStatus status { get; set; }  //  "active"
        public CultureInfo language { get; set; }  // "en_US" 
        public string meta_title { get; set; }  // null
        public string meta_description { get; set; }  //  null
        public long last_login { get; set; }  // null
        public long created_at { get; set; } // epoch time in milliseconds
        public int created_by { get; set; }  // the first user created has an id of 1
        public long updated_at { get; set; } // epoch time in milliseconds
        public int updated_by { get; set; }  // the first user created has an id of 1
    }

    public enum GhostUserStatus
    {
        active = 0,
        disabled
    }
}
