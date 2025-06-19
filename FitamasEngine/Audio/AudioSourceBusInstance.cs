using Microsoft.Xna.Framework;
using SoLoud;
using System;

namespace Fitamas.Audio
{
    public class AudioSourceBusInstance : AudioSourceInstance
    {
        internal Bus Bus;

        internal override SoloudObject SoloudObject => Bus;

        public AudioSourceBusInstance(AudioManager manager) : base(manager)
        {
            Bus = new Bus();
        }

        protected override uint Play(AudioObject audioObject)
        {
            if (audioObject == this)
            {
                return base.Play(audioObject);
            }

            if (audioObject.Is3d)
            {
                Vector3 position = audioObject.Position;
                Vector3 velocity = audioObject.Velocity;
                return Bus.play3d(audioObject.SoloudObject, position.X, position.Y, position.Z, velocity.X, velocity.Y, velocity.Z);
            }
            else
            {
                return Bus.play(audioObject.SoloudObject);
            }
        }

        protected override void SetFilter(uint filterId, SoloudObject soloudObject)
        {
            Bus.setFilter(filterId, soloudObject);
        }
    }
}
