using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.Physics;
using Fitamas.Physics.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Fitamas.DebugTools
{
    public class DebugRenderSystem : EntityDrawSystem
    {
        private GraphicsDevice graphicsDevice;

        private ComponentMapper<Transform> transformMapper;
        private ComponentMapper<Collider> colliderMapper;
        private ComponentMapper<SpriteRender> spriteMapper;
        private ComponentMapper<Mesh> meshMapper;
        private ComponentMapper<Character> characterMapper;

        public DebugRenderSystem(GraphicsDevice graphicsDevice) :
            base(Aspect.All(typeof(Transform))
                       .One(typeof(SpriteRender), typeof(Collider), typeof(Mesh), typeof(Character)))
        {
            this.graphicsDevice = graphicsDevice;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform>();
            colliderMapper = mapperService.GetMapper<Collider>();
            spriteMapper = mapperService.GetMapper<SpriteRender>();
            meshMapper = mapperService.GetMapper<Mesh>();
            characterMapper = mapperService.GetMapper<Character>();
        }

        public override void Draw(GameTime gameTime)
        {
            DebugGizmo.BeginBatch(graphicsDevice);

            foreach (var entityId in ActiveEntities)
            {
                Transform transform = transformMapper.Get(entityId);

                //Transform gizmo
                DebugGizmo.DrawAnchor(transform);

                //Sprite Rec
                if (spriteMapper.TryGet(entityId, out SpriteRender sprite))
                {
                    DebugGizmo.DrawSprite(transform, sprite);
                }

                //Polygon mesh
                if (meshMapper.TryGet(entityId, out Mesh mesh))
                {
                    DebugGizmo.DrawRenderMesh(transform, mesh);
                }

                //Colliders gizmo
                if (colliderMapper.TryGet(entityId, out Collider collider))
                {
                    DebugGizmo.DrawCollider(transform, collider);
                }

                //Character gizmo
                if (characterMapper.TryGet(entityId, out Character character))
                {
                    DebugGizmo.DrawCharacter(transform, character);
                }
            }

            DebugGizmo.EndBatch();




            ////RayCastHit[] hits = Physics2D.BoxCast(camera.ScreenToWorld(InputSystem.mouse.MousePosition), 2, 2);
            //Vector2 origin = camera.ScreenToWorld(InputSystem.mouse.MousePosition);
            //Vector2 direction = new Vector2(1, -1);
            //float distance = 1;
            ////float radius = 1;
            //Vector2 position = origin + direction.NormalizeF() * distance;
            ////RayCastHit[] hits = Physics2D.CircleCast(origin, direction, distance, radius);
            ////RayCastHit[] hits = Physics2D.OverlapCircle(origin, radius, false);
            //RayCastHit[] hits = Physics2D.OverlapBox(origin, 1, 1, 0);
            //foreach (var hit in hits)
            //{
            //    primitiveDrawing.DrawLine(hit.point, hit.point + hit.normal, Color.White, 0.02f);
            //    //Debug.Log(hit.distance);

            //    //Vector2 result = MathV.ProjectOnLine(pos, pos + direction, hit.point);

            //    //Vector2 normal = hit.normal;
            //    //Vector2 point = hit.point;
            //    //float resultDistance = 0;

            //    //primitiveDrawing.DrawRectangle()
            //    //primitiveDrawing.DrawCircle(origin + direction.NormalizeF() * hit.distance, 1, Color.Green);
            //    //primitiveDrawing.DrawCircle(origin + direction.NormalizeF() * hit.distance, 1, Color.Green);


            //}
            //primitiveDrawing.DrawRectangle(origin - Vector2.One / 2, 1, 1, Color.Green);
            ////primitiveDrawing.DrawCircle(origin, 1, Color.Green);
            //primitiveDrawing.DrawSegment(origin, origin + direction.NormalizeF() * 1, Color.Red);
        }
    }
}
