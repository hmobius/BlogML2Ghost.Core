// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostImportDocument
    {
        public GhostMetadata meta { get; set; }    
        public GhostData data { get; set; }
    }
}