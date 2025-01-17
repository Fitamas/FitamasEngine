using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input
{
    public enum PlayerType
    {
        None,
        User,
        Ai,
        Network
    }

    public class InputRequest
    {
        public PlayerType PlayerType;
        public Vector2 MoveDirection { get; set; }
        public Vector2 Target { get; set; }
        public bool IsRagDoll { get; set; }

        public InputRequest()
        {

        }

        public InputRequest(PlayerType playerType)
        {
            PlayerType = playerType;
        }
    }
}
