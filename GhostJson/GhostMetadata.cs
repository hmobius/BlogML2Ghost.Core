// Derived from https://docs.ghost.org/docs/the-importer#section-rolling-your-own

using System;
using BlogML2Ghost.Core.ExtensionMethods;

namespace BlogML2Ghost.Core.GhostJson
{
    public class GhostMetadata
    {
        readonly string currentVersion = "1.22.0";

        public long exported_on { get; private set; }

        public string version { get; private set; }

        public GhostMetadata()
        {
            exported_on = DateTime.UtcNow.AsMillisecondsSinceEpoch();
            version = currentVersion;
        }
    }
}
