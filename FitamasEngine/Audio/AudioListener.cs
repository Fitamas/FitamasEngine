using Fitamas.ECS;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Audio
{
    public class AudioListener : Component
    {
        public AudioListenerInstance Instance { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Velocity { get; set; }

        public AudioListener()
        {
            Position = Vector3.Zero;
            Forward = Vector3.Forward;
            Up = Vector3.Up;
            Velocity = Vector3.Zero;
        }
    }
}
