using Fitamas.Serializeble;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Graphics
{
    public class Matireal : MonoObject
    {
        [SerializableField] private Texture2D texture;

        public Texture2D Texture => texture;

        public Matireal(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
