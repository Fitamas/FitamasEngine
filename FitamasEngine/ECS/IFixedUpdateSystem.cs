﻿using Fitamas.ECS;

namespace Fitamas.Extended.Entities
{
    public interface IFixedUpdateSystem : ISystem
    {
        public void FixedUpdate(float deltaTime);
    }
}
