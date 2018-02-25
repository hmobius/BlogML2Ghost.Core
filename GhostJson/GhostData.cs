// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own

using System.Collections.Generic;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostData
    {
        readonly int maxSlugLength = 191;

        public List<GhostPost> posts { get; set; }

        public List<GhostTag> tags { get; set; }

        public List<GhostPostHasTag> posts_tags { get; set;}

        public List<GhostUser> users { get; set; }

        public List<GhostUserHasRole> roles_users { get; set; }
    }
}
