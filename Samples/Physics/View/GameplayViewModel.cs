using Fitamas;
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
        private Tool tool;
        private bool use;

        public bool CanUse;

        public GameplayViewModel(GameWorld world)
        {
            this.world = world;
        }

        public void SelectTool(Tool tool)
        {
            this.tool = tool;
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

                        RayCastHit hit = Physics2D.TestPoint(position);

                        if (hit.Entity != null)
                        {
                            world.DestroyEntity(hit.Entity);
                        }
                        break;
                    case Tool.RopeJoint:
                        PhysicsDebugTools.CreateRopeJoint(position);
                        break;
                    case Tool.RevoltJoint:
                        PhysicsDebugTools.CreateRevoltJoint(position);
                        break;
                    case Tool.WheelJoint:
                        PhysicsDebugTools.CreateWheelJoint(position);
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
    }
}
