using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Actions
{
    public struct InputActionState
    {
        private bool isActive;
        private InputActionType type;
        private TimeSpan startTime;

        public bool IsActive => isActive;
        public InputActionType Type => type;
        public TimeSpan StartTime => startTime;
        public bool InProgress => type != InputActionType.None;
        public bool IsStarted => InProgress && type.HasFlag(InputActionType.Started);
        public bool IsPerformed => InProgress && type.HasFlag(InputActionType.Performed);
        public bool IsCanceled => InProgress && type.HasFlag(InputActionType.Canceled);

        public void Started(GameTime gameTime)
        {
            if (!isActive)
            {
                startTime = gameTime.TotalGameTime;
                type = InputActionType.Started;
                isActive = true;
            }
        }

        public void Performed(GameTime gameTime)
        {
            if (isActive)
            {
                type |= InputActionType.Performed;
            }
            else
            {
                startTime = gameTime.TotalGameTime;
                type = InputActionType.Started | InputActionType.Performed;
                isActive = true;
            }
        }

        public void Canceled()
        {
            type |= InputActionType.Canceled;
            isActive = false;
        }

        public void Flush()
        {
            type = InputActionType.None;
        }
    }
}
