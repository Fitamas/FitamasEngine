/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Fitamas;

namespace MonoGame.Extended.Input.InputListeners
{
    public class InputListenerComponent : GameComponent, IUpdate
    {
        private readonly List<InputListener> _listeners;

        public InputListenerComponent(Game game)
            : base(game)
        {
            _listeners = new List<InputListener>();
        }

        public InputListenerComponent(Game game, params InputListener[] listeners)
            : base(game)
        {
            _listeners = new List<InputListener>(listeners);
        }

        public IList<InputListener> Listeners => _listeners;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Game.IsActive)
            {
                foreach (var listener in _listeners)
                    listener.Update(gameTime);
            }

            GamePadListener.CheckConnections();
        }
    }
}