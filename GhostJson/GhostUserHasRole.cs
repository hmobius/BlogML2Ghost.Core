// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own
// Reflects the information that links roles to users in ghost.json

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostUserHasRole
    {
        public GhostUserHasRole(int userId, int roleId)
        {
            user_id = userId;
            role_id = roleId;
        }

        public int user_id { get; set; }
        public int role_id { get; set; }
    }
}
