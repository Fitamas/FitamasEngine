using Fitamas.Entities;
using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Fitamas.Serializeble;
using Fitamas.Container;

namespace Fitamas.Main
{
    public class GameMain : Game
    {
        private GameWorld world;

        public GameWorld World => world;

        public DIContainer Container { get; }
        private ObjectManager ObjectManager { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        public GameMain(GameEntryPoint entryPoint)
        {
            Container = new DIContainer();

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            ObjectManager = new ObjectManager(this, "Content");

            entryPoint.LoadSettings(this, Container);

            Debug.Log("Initialize systems");

            world = entryPoint.CreateWorld(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Debug.Log("Load content");

            Resources.LoadContent(ObjectManager);

            world.LoadContent(Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Camera.Current = Camera.Main;

            world.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            world.Dispose();
        }
    }
}
