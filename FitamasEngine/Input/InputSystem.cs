using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Main;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;

namespace Fitamas.Input
{
    public class InputSystem : EntityUpdateSystem
    {
        public static KeyboardListener keyboard { get; private set; }
        public static MouseListener mouse { get; private set; }
        public static ActionMaps actionMaps { get; private set; }
        public static Point mouseDelta { get; private set; }
        public static Vector2 mousePositionInWorld { get; private set; }

        private ComponentMapper<InputRequest> inputMapper;
        private Camera camera;
        private Point mousePosition;

        public InputSystem() : base(Aspect.All(typeof(InputRequest)))
        {

        }

        public override void Initialize(GameWorld world)
        {
            KeyboardListenerSettings settings = new KeyboardListenerSettings();
            settings.InitialDelayMilliseconds = 100;

            keyboard = new KeyboardListener(settings);
            mouse = new MouseListener();

            InputListenerComponent component = new InputListenerComponent(world.Game, mouse, keyboard);
            world.Game.Components.Add(component);

            camera = Camera.Main;
            actionMaps = new ActionMaps(mouse, keyboard);

            base.Initialize(world);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            inputMapper = mapperService.GetMapper<InputRequest>();
        }

        public override void Update(GameTime gameTime)
        {
            actionMaps.Update();

            mouseDelta = mousePosition - mouse.MousePosition;
            mousePosition = mouse.MousePosition;

            if (camera != null)
            {
                mouse.ViewportAdapter = camera.ViewportAdapter;
                mousePositionInWorld = camera.ScreenToWorld(mouse.MousePosition);
            }

            foreach (var id in ActiveEntities)
            {
                InputRequest input = inputMapper.Get(id);
                if (input.PlayerType == PlayerType.User)
                {
                    input.Target = mousePositionInWorld;
                    input.MoveDirection = actionMaps.Direction.Value;

                    if (actionMaps.DropItem.IsTap)
                    {
                        input.IsRagDoll = !input.IsRagDoll;
                    }
                }
            }
        }
    }
}
