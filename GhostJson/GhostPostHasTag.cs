// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that can associates tags with posts in ghost.json

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostPostHasTag
    {
        public GhostPostHasTag(int tagId, int postId)
        {
            tag_id = tagId;
            post_id = postId;
        }

        public int tag_id { get; set; }
        public int post_id { get; set; }
    }
}
