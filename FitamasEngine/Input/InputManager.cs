using Fitamas.Core;
using Fitamas.Input.Actions;
using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fitamas.Input
{
    public class InputManager
    {
        private InputListenerComponent inputListenerComponent;
        private InputActionMapComponent inputActionMapComponent;

        public KeyboardListener Keyboard { get; }
        public MouseListener Mouse { get; }

        public IList<InputListener> Listeners => inputListenerComponent.Listeners;

        public InputManager(GameEngine game)
        {
            KeyboardListenerSettings settings = new KeyboardListenerSettings();
            settings.InitialDelayMilliseconds = 0;
            settings.RepeatDelayMilliseconds = 0;
            Keyboard = new KeyboardListener(settings);
            Mouse = new MouseListener(game.WindowViewportAdapter);

            inputListenerComponent = new InputListenerComponent(game, Mouse, Keyboard);
            game.Components.Add(inputListenerComponent);

            inputActionMapComponent = new InputActionMapComponent(game);
            game.Components.Add(inputActionMapComponent);
        }

        public void AddActionMap(InputActionMap actionMap)
        {
            actionMap.InputManager = this;
            inputActionMapComponent.ActionMaps.Add(actionMap);
        }
    }
}
