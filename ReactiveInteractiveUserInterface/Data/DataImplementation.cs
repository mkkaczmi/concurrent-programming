﻿//____________________________________________________________________________________________________________________________________
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
            MoveTimer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(50));

            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                IVector startingPosition = new Vector(random.Next(0, (int)GetDimensions.TableWidth), random.Next(0, (int)GetDimensions.TableHeight));
                IVector initialVelocity = new Vector((random.NextDouble() * 4 - 2) * 2, (random.NextDouble() * 4 - 2) * 2);
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

        public override void UpdateBoxDimensions(double width, double height)
        {
            boxWidth = width;
            boxHeight = height;
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
        private double boxWidth = 400;
        private double boxHeight = 400;

        private void Move(object? x)
        {
            foreach (Ball item in BallsList)
            {
                // Get current position and velocity
                Vector currentPos = (Vector)item.Position;
                Vector currentVel = (Vector)item.Velocity;

                // Calculate new position
                double newX = currentPos.x + currentVel.x;
                double newY = currentPos.y + currentVel.y;

                // Handle collisions with walls
                // Left wall collision
                if (newX < 0)
                {
                    currentVel = new Vector(Math.Abs(currentVel.x), currentVel.y);
                    newX = 0;
                }
                // Right wall collision
                else if (newX + GetDimensions.BallDimension > boxWidth - 8) // Account for border thickness
                {
                    currentVel = new Vector(-Math.Abs(currentVel.x), currentVel.y);
                    newX = boxWidth - GetDimensions.BallDimension - 8; // Account for border thickness
                }

                // Top wall collision
                if (newY < 0)
                {
                    currentVel = new Vector(currentVel.x, Math.Abs(currentVel.y));
                    newY = 0;
                }
                // Bottom wall collision
                else if (newY + GetDimensions.BallDimension > boxHeight - 8) // Account for border thickness
                {
                    currentVel = new Vector(currentVel.x, -Math.Abs(currentVel.y));
                    newY = boxHeight - GetDimensions.BallDimension - 8; // Account for border thickness
                }

                // Update velocity and move the ball
                item.Velocity = currentVel;
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