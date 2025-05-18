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
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation()
        {
            MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            // Ensure the timer is running
            MoveTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                IVector startingPosition = new Vector(random.Next(0, (int)GetDimensions.TableWidth), random.Next(0, (int)GetDimensions.TableHeight));
                IVector initialVelocity = new Vector(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
                IBall ball = new Ball(startingPosition, initialVelocity);
                BallsList.Add((Ball)ball);
                upperLayerHandler(startingPosition, ball);
            }
        }

        public override void Stop()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            MoveTimer.Change(Timeout.Infinite, Timeout.Infinite);
            BallsList.Clear();
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    MoveTimer.Dispose();
                    BallsList.Clear();
                }
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(nameof(DataImplementation));
        }

        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        //private bool disposedValue;
        private bool Disposed = false;

        private readonly Timer MoveTimer;
        private Random RandomGenerator = new();
        private List<Ball> BallsList = [];

        private void Move(object? x)
        {
            foreach (Ball item in BallsList)
            {
                // Generate random movement within bounds
                double deltaX = (RandomGenerator.NextDouble() - 0.5) * 10;
                double deltaY = (RandomGenerator.NextDouble() - 0.5) * 10;

                // Get current position
                Vector currentPos = (Vector)item.Velocity;

                // Calculate new position
                double newX = currentPos.x + deltaX;
                double newY = currentPos.y + deltaY;

                // Ensure the ball stays within the confined area (400x400)
                newX = Math.Max(0, Math.Min(400 - 20, newX)); // 20 is the ball diameter
                newY = Math.Max(0, Math.Min(400 - 20, newY));

                // Move the ball
                item.Move(new Vector(newX - currentPos.x, newY - currentPos.y));
            }
        }

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
        {
            returnBallsList(BallsList);
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
        {
            returnNumberOfBalls(BallsList.Count);
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}