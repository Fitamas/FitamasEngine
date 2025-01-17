using ClipperLib;
using Fitamas.Entities;
using Fitamas.Input;
using Fitamas.Physics;
using Microsoft.Xna.Framework;

namespace Fitamas.Gameplay.Characters
{
    public class PlayerSystem : EntityUpdateSystem
    {
        private ComponentMapper<Character> characterMapper;
        private ComponentMapper<InputRequest> inputMapper;

        public PlayerSystem() : base(Aspect.All(typeof(Character), typeof(InputRequest)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            characterMapper = mapperService.GetMapper<Character>();
            inputMapper = mapperService.GetMapper<InputRequest>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var id in ActiveEntities)
            {
                InputRequest input = inputMapper.Get(id);
                Character character = characterMapper.Get(id);

                if (input.MoveDirection.LengthSquared() > 0)
                {
                    character.velocity = new Vector2(input.MoveDirection.X * character.moveSpeed, 0);
                    character.characterState = CharacterState.walk;
                }
                else
                {
                    character.velocity = Vector2.Zero;
                    character.characterState = CharacterState.idle;
                }


                character.isRagDoll = input.IsRagDoll;
            }
        }
    }
}
