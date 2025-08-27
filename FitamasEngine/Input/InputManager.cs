using Fitamas.Core;
using Fitamas.Input.Actions;
using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fitamas.Input
{
    public class InputManager : Core.IUpdateable
    {
        private GameEngine game;
        private List<InputActionMap> actionMaps;
        private List<InputListener> listeners;

        public bool IsActive { get; set; }
        public KeyboardListener Keyboard { get; }
        public MouseListener Mouse { get; }

        public IList<InputListener> Listeners => listeners;

        public InputManager(GameEngine game)
        {
            this.game = game;

            IsActive = true;
            KeyboardListenerSettings settings = new KeyboardListenerSettings();
            settings.InitialDelayMilliseconds = 0;
            settings.RepeatDelayMilliseconds = 0;
            Keyboard = new KeyboardListener(settings);
            Mouse = new MouseListener(game);
            listeners = new List<InputListener>()
            {
                Keyboard,
                Mouse,
            };
            actionMaps = new List<InputActionMap>();
        }

        public void AddActionMap(InputActionMap actionMap)
        {
            actionMap.InputManager = this;
            actionMaps.Add(actionMap);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            if (game.IsActive)
            {
                foreach (var listener in listeners)
                {
                    listener.Update(gameTime);
                }
            }

            GamePadListener.CheckConnections();

            foreach (var map in actionMaps)
            {
                map.Update(gameTime);
            }
        }
    }
}
