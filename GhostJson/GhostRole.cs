using System;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostRole
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int created_by { get; set; }  // user id - probably 1
        public DateTime updated_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int updated_by { get; set; }  // user id- probably 1
    }
}