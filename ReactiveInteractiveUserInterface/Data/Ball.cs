//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(IVector initialPosition, IVector initialVelocity)
        {
            position = initialPosition;
            velocity = initialVelocity;
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        #endregion IBall

        #region private

        private IVector position;
        private IVector velocity;

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, position);
        }

        internal void Move(IVector delta)
        {
            position = new Vector(position.x + delta.x, position.y + delta.y);
            velocity = position; // Update velocity to match position for boundary checking
            RaiseNewPositionChangeNotification();
        }

        #endregion private
    }
}