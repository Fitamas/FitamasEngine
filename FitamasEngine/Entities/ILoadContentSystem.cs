using Fitamas.Entities;
using Microsoft.Xna.Framework.Content;

namespace Fitamas.Extended.Entities
{
    public interface ILoadContentSystem : ISystem
    {
        void LoadContent(ContentManager content);
    }
}
