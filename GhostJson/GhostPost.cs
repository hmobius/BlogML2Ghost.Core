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
        public string mobiledoc { get; set; }  // markdown version of the post
        public string html { get; set; }  // html version of the post
        public string amp { get; set; } // null
        public string plaintext { get; set; } // plaintext version of the post
        public string feature_image { get; set; }  // null
        public bool featured { get; set; }  // set to true if the post was featured, false if not
        public bool page { get; set; }  // set true if this is a page, false if a post
        public GhostPostStatus status { get; set; }  // set to published or draft
        public CultureInfo locale { get; set; }  // language string for post - en-US, en-GB etc
        public GhostVisibility visibility { get; set; } // whether post is visible or not
        public string meta_title { get; set; }  // null
        public string meta_description { get; set; }  //  null
        public int author_id { get; set; }  // the first user created has an id of 1
        public DateTime created_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int created_by { get; set; }  // the first user created has an id of 1
        public DateTime updated_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int updated_by { get; set; }  // the first user created has an id of 1
        public DateTime published_at { get; set; } // as yyyy-MM-dd hh:mm:ss
        public int published_by { get; set; }  // the first user created has an id of 1
        public string custom_excerpt { get; set; } //null
        public string codeinjection_head { get; set; } // null
        public string codeinjection_foot { get; set; } // null
        public string og_image { get; set; } // null
        public string og_title { get; set; } // null
        public string og_description { get; set; } //null
        public string twitter_image { get; set; } // null
        public string twitter_title { get; set; } // null
        public string  twitter_description { get; set; } // null
        public string custom_template { get; set; } // null
    }

    public enum GhostPostStatus
    {
        published = 0,
        draft
    }

    public enum GhostVisibility
    {
        @public = 0,
        @private
    }
}
