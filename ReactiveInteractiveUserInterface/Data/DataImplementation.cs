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
        private const double GRAVITY = 0.0; // No gravity as per requirements
        private const double FRICTION = 0.0; // No friction as per requirements

        private void Move(object? x)
        {
            // First, update positions and handle wall collisions
            foreach (Ball ball in BallsList)
            {
                UpdateBallPosition(ball);
            }

            // Then handle ball-to-ball collisions
            HandleBallCollisions();
        }

        private void UpdateBallPosition(Ball ball)
        {
            Vector currentPos = (Vector)ball.Position;
            Vector currentVel = (Vector)ball.Velocity;

            // Calculate new position with physics
            double newX = currentPos.x + currentVel.x;
            double newY = currentPos.y + currentVel.y;

            // Handle wall collisions (elastic)
            if (newX < 0)
            {
                currentVel = new Vector(Math.Abs(currentVel.x), currentVel.y);
                newX = 0;
            }
            else if (newX + GetDimensions.BallDimension > boxWidth - 8)
            {
                currentVel = new Vector(-Math.Abs(currentVel.x), currentVel.y);
                newX = boxWidth - GetDimensions.BallDimension - 8;
            }

            if (newY < 0)
            {
                currentVel = new Vector(currentVel.x, Math.Abs(currentVel.y));
                newY = 0;
            }
            else if (newY + GetDimensions.BallDimension > boxHeight - 8)
            {
                currentVel = new Vector(currentVel.x, -Math.Abs(currentVel.y));
                newY = boxHeight - GetDimensions.BallDimension - 8;
            }

            // Update velocity and position
            ball.Velocity = currentVel;
            ball.Move(new Vector(newX - currentPos.x, newY - currentPos.y));
        }

        private void HandleBallCollisions()
        {
            for (int i = 0; i < BallsList.Count; i++)
            {
                for (int j = i + 1; j < BallsList.Count; j++)
                {
                    Ball ball1 = BallsList[i];
                    Ball ball2 = BallsList[j];

                    Vector pos1 = (Vector)ball1.Position;
                    Vector pos2 = (Vector)ball2.Position;
                    Vector vel1 = (Vector)ball1.Velocity;
                    Vector vel2 = (Vector)ball2.Velocity;

                    // Calculate distance between ball centers
                    double dx = pos2.x - pos1.x;
                    double dy = pos2.y - pos1.y;
                    double distanceSquared = dx * dx + dy * dy;
                    double minDistance = GetDimensions.BallDimension;

                    // Check if balls are colliding
                    if (distanceSquared < minDistance * minDistance)
                    {
                        double distance = Math.Sqrt(distanceSquared);

                        // Normalize collision vector
                        double nx = dx / distance;
                        double ny = dy / distance;

                        // Calculate relative velocity
                        double relativeVelocityX = vel2.x - vel1.x;
                        double relativeVelocityY = vel2.y - vel1.y;

                        // Calculate relative velocity in terms of the normal direction
                        double velocityAlongNormal = relativeVelocityX * nx + relativeVelocityY * ny;

                        // Do not resolve if velocities are separating
                        if (velocityAlongNormal > 0)
                            continue;

                        // Calculate impulse scalar with mass consideration
                        double restitution = 1.0; // Perfectly elastic collision
                        double mass1 = ball1.Mass;
                        double mass2 = ball2.Mass;
                        double totalMass = mass1 + mass2;
                        
                        // Calculate impulse scalar using conservation of momentum and energy
                        double impulseScalar = -(1 + restitution) * velocityAlongNormal * (mass1 * mass2) / totalMass;

                        // Apply impulse with mass consideration
                        Vector newVel1 = new Vector(
                            vel1.x - (impulseScalar * nx) / mass1,
                            vel1.y - (impulseScalar * ny) / mass1
                        );
                        Vector newVel2 = new Vector(
                            vel2.x + (impulseScalar * nx) / mass2,
                            vel2.y + (impulseScalar * ny) / mass2
                        );

                        // Update velocities
                        ball1.Velocity = newVel1;
                        ball2.Velocity = newVel2;

                        // Move balls apart to prevent sticking
                        double overlap = minDistance - distance;
                        double moveX = overlap * nx / 2;
                        double moveY = overlap * ny / 2;

                        // Apply position correction
                        ball1.Move(new Vector(-moveX, -moveY));
                        ball2.Move(new Vector(moveX, moveY));
                    }
                }
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