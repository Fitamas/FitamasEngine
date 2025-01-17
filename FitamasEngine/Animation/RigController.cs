using Fitamas.Physics.Characters;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class RigController
    {
        protected List<Rig> rigs = new List<Rig>();
        protected Avatar avatar;

        public Avatar Avatar => avatar;

        public void SetAvatar(Avatar avatar)
        {
            this.avatar = avatar;
        }

        public void Update(float deltaTime)
        {
            foreach (var rig in rigs)
            {
                rig.Update(deltaTime);
            }
        }

        public void AddRig(Rig rig)
        {
            rigs.Add(rig);
        }
    }
}
