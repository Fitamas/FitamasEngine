using Fitamas;
using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.MVVM;
using Fitamas.Physics;
using Microsoft.Xna.Framework;
using Physics.Gameplay;
using System;

namespace Physics.View
{
    public enum Tool
    {
        None,
        Pumpkin,
        Log,
        TestBox,
        Wheel,
        Destroy,
        RopeJoint,
        RevoltJoint,
        WheelJoint,
    }

    public class GameplayViewModel : IViewModel
    {
        private GameWorld world;
        private PhysicsWorldSystem physicsWorld;
        private Tool tool;
        private bool use;

        private Entity entityA;
        private Vector2 positionA;

        public bool CanUse;

        public GameplayViewModel(GameEngine game)
        {
            world = game.World;
            physicsWorld = game.MainContainer.Resolve<PhysicsWorldSystem>();
        }

        public void SelectTool(Tool tool)
        {
            this.tool = tool;
            entityA = null;
        }

        public void BeginUseTool(Point point)
        {
            if (!CanUse)
            {
                return;
            }

            if (!use)
            {
                Vector2 position = Camera.Main.ScreenToWorld(point);

                switch (tool)
                {
                    case Tool.None:
                        PhysicsDebugTools.CreateMousJoint(position);
                        break;
                    case Tool.Pumpkin:
                        EntityHelper.CreatePumpkin(world, position);
                        break;
                    case Tool.Log:
                        EntityHelper.CreateLog(world, position);
                        break;
                    case Tool.TestBox:
                        EntityHelper.CreateTestBox(world, position);
                        break;
                    case Tool.Wheel:
                        EntityHelper.CreateWheel(world, position);
                        break;
                    case Tool.Destroy:
                        RayCastHit hit = physicsWorld.TestPoint(position);

                        if (hit.Entity != null)
                        {
                            world.DestroyEntity(hit.Entity);
                        }
                        break;
                    case Tool.RopeJoint:
                        CreateRopeJoint(position);
                        break;
                    case Tool.RevoltJoint:
                        CreateRevoltJoint(position);
                        break;
                    case Tool.WheelJoint:
                        CreateWheelJoint(position);
                        break;
                }
            }
            use = true;
        }

        public void UseTool(Point point)
        {
            if (!CanUse)
            {
                return;
            }

            if (use)
            {
                Vector2 position = Camera.Main.ScreenToWorld(point);

                switch (tool)
                {
                    case Tool.None:
                        PhysicsDebugTools.SetMousePosition(position);
                        break;
                }
            }
        }

        public void EndUseTool(Point position)
        {
            if (!CanUse)
            {
                return;
            }

            if (use)
            {
                switch (tool)
                {
                    case Tool.None:
                        PhysicsDebugTools.RemoveMouseJoint();
                        break;
                }
            }
            use = false;
        }



        public void CreateRopeJoint(Vector2 position)
        {
            RayCastHit hit = physicsWorld.TestPoint(position);

            if (hit.Entity != null)
            {
                if (entityA == null)
                {
                    entityA = hit.Entity;
                    positionA = position;
                }
                else if (entityA != hit.Entity)
                {
                    Entity entity = world.CreateEntity();

                    entity.Attach(PhysicsJoint.CreateRope(entityA, positionA, hit.Entity, position, Vector2.Distance(positionA, position), true));

                    entityA = null;
                }
            }
        }

        public void CreateRevoltJoint(Vector2 position)
        {
            RayCastHit hit = physicsWorld.TestPoint(position);

            if (hit.Entity != null)
            {
                if (entityA == null)
                {
                    entityA = hit.Entity;
                    positionA = position;
                }
                else if (entityA != hit.Entity)
                {
                    Entity entity = world.CreateEntity();

                    entity.Attach(PhysicsJoint.CreateRevolt(entityA, positionA, hit.Entity, position, true));

                    entityA = null;
                }
            }
        }

        public void CreateWheelJoint(Vector2 position)
        {
            RayCastHit hit = physicsWorld.TestPoint(position);

            if (hit.Entity != null)
            {
                Entity entity = world.CreateEntity();

                entity.Attach(PhysicsJoint.CreateWheel(hit.Entity, Vector2.Zero, EntityHelper.CreateWheel(world, position), new Vector2(0, 1)));
            }
        }
    }
}
