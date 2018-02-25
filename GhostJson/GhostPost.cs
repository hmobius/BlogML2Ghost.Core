// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can be stored about a post in ghost.json

using System;
using System.Globalization;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostPost
    {
        public int id { get; set; }
        public string title { get; set; }   // "my blog post title"
        public string slug { get; set; }  // "my-blog-post-title" - max allowed limit of 191 chars
        public string markdown { get; set; }  // markdown version of the post
        public string html { get; set; }  // html version of the post
        public string image { get; set; }  // null
        public bool featured { get; set; }  // set to true if the post was featured, false if not
        public bool page { get; set; }  // set true if this is a page, false if a post
        public GhostPostStatus status { get; set; }  // set to published or draft
        public CultureInfo language { get; set; }  // language string for post - en-US, en-GB etc
        public string meta_title { get; set; }  // null
        public string meta_description { get; set; }  //  null
        public int author_id { get; set; }  // the first user created has an id of 1
        public long created_at { get; set; } // epoch time in milliseconds
        public int created_by { get; set; }  // the first user created has an id of 1
        public long updated_at { get; set; } // epoch time in milliseconds
        public int updated_by { get; set; }  // the first user created has an id of 1
        public long published_at { get; set; } // epoch time in milliseconds
        public int published_by { get; set; }  // the first user created has an id of 1
    }

    public enum GhostPostStatus
    {
        published = 0,
        draft
    }
}
