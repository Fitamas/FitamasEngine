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

using Microsoft.Xna.Framework;

namespace Fitamas.Input.InputListeners
{
    /// <summary>
    ///     This is a class that contains settings to be used to initialise a <see cref="GamePadListener" />.
    /// </summary>
    /// <seealso cref="InputListenerManager" />
    public class GamePadListenerSettings : InputListenerSettings<GamePadListener>
    {
        public GamePadListenerSettings()
            : this(PlayerIndex.One)
        {
        }

        /// <summary>
        ///     This is a class that contains settings to be used to initialise a <see cref="GamePadListener" />.
        ///     <para>Note: There are a number of extra settings that are settable properties.</para>
        /// </summary>
        /// <param name="playerIndex">The index of the controller the listener will be tied to.</param>
        /// <param name="vibrationEnabled">Whether vibration is enabled on the controller.</param>
        /// <param name="vibrationStrengthLeft">
        ///     General setting for the strength of the left motor.
        ///     This motor has a slow, deep, powerful rumble.
        ///     This setting will modify all future vibrations
        ///     through this listener.
        /// </param>
        /// <param name="vibrationStrengthRight">
        ///     General setting for the strength of the right motor.
        ///     This motor has a snappy, quick, high-pitched rumble.
        ///     This setting will modify all future vibrations
        ///     through this listener.
        /// </param>
        public GamePadListenerSettings(PlayerIndex playerIndex, bool vibrationEnabled = true,
            float vibrationStrengthLeft = 1.0f, float vibrationStrengthRight = 1.0f)
        {
            PlayerIndex = playerIndex;
            VibrationEnabled = vibrationEnabled;
            VibrationStrengthLeft = vibrationStrengthLeft;
            VibrationStrengthRight = vibrationStrengthRight;
            TriggerDownTreshold = 0.15f;
            ThumbstickDownTreshold = 0.5f;
            RepeatInitialDelay = 500;
            RepeatDelay = 50;
        }

        /// <summary>
        ///     The index of the controller.
        /// </summary>
        public PlayerIndex PlayerIndex { get; set; }

        /// <summary>
        ///     When a button is held down, the interval in which
        ///     ButtonRepeated fires. Value in milliseconds.
        /// </summary>
        public int RepeatDelay { get; set; }

        /// <summary>
        ///     The amount of time a button has to be held down
        ///     in order to fire ButtonRepeated the first time.
        ///     Value in milliseconds.
        /// </summary>
        public int RepeatInitialDelay { get; set; }


        /// <summary>
        ///     Whether vibration is enabled for this controller.
        /// </summary>
        public bool VibrationEnabled { get; set; }

        /// <summary>
        ///     General setting for the strength of the left motor.
        ///     This motor has a slow, deep, powerful rumble.
        ///     <para>
        ///         This setting will modify all future vibrations
        ///         through this listener.
        ///     </para>
        /// </summary>
        public float VibrationStrengthLeft { get; set; }

        /// <summary>
        ///     General setting for the strength of the right motor.
        ///     This motor has a snappy, quick, high-pitched rumble.
        ///     <para>
        ///         This setting will modify all future vibrations
        ///         through this listener.
        ///     </para>
        /// </summary>
        public float VibrationStrengthRight { get; set; }

        /// <summary>
        ///     The treshold of movement that has to be met in order
        ///     for the listener to fire an event with the trigger's
        ///     updated position.
        ///     <para>
        ///         In essence this defines the event's
        ///         resolution.
        ///     </para>
        ///     At a value of 0 this will fire every time
        ///     the trigger's position is not 0f.
        /// </summary>
        public float TriggerDeltaTreshold { get; set; }

        /// <summary>
        ///     The treshold of movement that has to be met in order
        ///     for the listener to fire an event with the thumbstick's
        ///     updated position.
        ///     <para>
        ///         In essence this defines the event's
        ///         resolution.
        ///     </para>
        ///     At a value of 0 this will fire every time
        ///     the thumbstick's position is not {x:0, y:0}.
        /// </summary>
        public float ThumbStickDeltaTreshold { get; set; }

        /// <summary>
        ///     How deep the triggers have to be depressed in order to
        ///     register as a ButtonDown event.
        /// </summary>
        public float TriggerDownTreshold { get; set; }

        /// <summary>
        ///     How deep the triggers have to be depressed in order to
        ///     register as a ButtonDown event.
        /// </summary>
        public float ThumbstickDownTreshold { get; private set; }

        public override GamePadListener CreateListener()
        {
            return new GamePadListener(this);
        }
    }
}