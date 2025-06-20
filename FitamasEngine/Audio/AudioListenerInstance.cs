using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Audio
{
    public class AudioListenerInstance
    {
        private AudioManager manager;

        public AudioListenerInstance(AudioManager manager)
        {
            this.manager = manager;
        }

        public void SetPosition(Vector3 position)
        {
            manager.Soloud.set3dListenerPosition(position.X, position.Y, position.Z);
        }

        public void SetForward(Vector3 forward)
        {
            manager.Soloud.set3dListenerAt(forward.X, forward.Y, forward.Z);
        }

        public void SetUp(Vector3 up)
        {
            manager.Soloud.set3dListenerUp(up.X, up.Y, up.Z);
        }

        public void SetVeclocity(Vector3 velocity)
        {
            manager.Soloud.set3dListenerVelocity(velocity.X, velocity.Y, velocity.Z);
        }
    }
}
