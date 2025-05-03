using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Fitamas.Serialization
{
    public static class ContentExtensions
    {
        public static Stream OpenStream(this ContentManager manager, string path)
        {
            return TitleContainer.OpenStream(Path.Combine(manager.RootDirectory, path));
        }
    }
}
