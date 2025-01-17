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

using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Extended.Input
{
    public struct KeyboardStateExtended
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public KeyboardStateExtended(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            _currentKeyboardState = currentKeyboardState;
            _previousKeyboardState = previousKeyboardState;
        }

        public bool CapsLock => _currentKeyboardState.CapsLock;
        public bool NumLock => _currentKeyboardState.NumLock;
        public bool IsShiftDown() => _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
        public bool IsControlDown() => _currentKeyboardState.IsKeyDown(Keys.LeftControl) || _currentKeyboardState.IsKeyDown(Keys.RightControl);
        public bool IsAltDown() => _currentKeyboardState.IsKeyDown(Keys.LeftAlt) || _currentKeyboardState.IsKeyDown(Keys.RightAlt);
        public bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public Keys[] GetPressedKeys() => _currentKeyboardState.GetPressedKeys();

        public bool WasKeyJustDown(Keys key) => _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        public bool WasKeyJustUp(Keys key) => _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);
        public bool WasAnyKeyJustDown() => _previousKeyboardState.GetPressedKeys().Any();
   }
}