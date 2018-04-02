// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own

using System.Collections.Generic;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostData
    {
        public List<GhostPost> posts { get; set; } = new List<GhostPost>();
        public List<GhostUser> users { get; set; } = new List<GhostUser>();
        public List<GhostPostHasAuthor> posts_authors  { get; set; } = new List<GhostPostHasAuthor>();
        public List<GhostRole> roles { get; set; }
        public List<GhostUserHasRole> roles_users { get; set; } = new List<GhostUserHasRole>();
        public List<GhostTag> tags { get; set; } = new List<GhostTag>();
        public List<GhostPostHasTag> posts_tags { get; set;} = new List<GhostPostHasTag>();
    }
}
