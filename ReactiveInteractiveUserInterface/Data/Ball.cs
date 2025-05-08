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

namespace TP.ConcurrentProgramming.Data.Impl      // warstwa implementacyjna
{
    /// <summary>
    /// Reprezentacja pojedynczej kuli znajdującej się na planszy.
    /// Klasa jest hermetyczna (sealed) i spełnia abstrakcyjny kontrakt <see cref="IBall"/>.
    /// </summary>
    internal sealed class Ball : IBall
    {
        /* ----------  ctor  ---------- */
        internal Ball(Guid id, double x, double y, double radius)
        {
            Id = id;
            X = x;
            Y = y;
            Radius = radius;
        }

        /* ----------  IBall ---------- */
        public Guid Id { get; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Radius { get; }

        public event EventHandler<BallMovedEventArgs>? NewPositionNotification;

        /* ----------  public API ---------- */
        /// <summary>
        /// Przesuwa kulę o <paramref name="dx"/>, <paramref name="dy"/> i publikuje zdarzenie z nową pozycją.
        /// </summary>
        internal void Move(double dx, double dy)
        {
            X += dx;
            Y += dy;
            RaiseNewPositionNotification();
        }

        /* ----------  helpers ---------- */
        private void RaiseNewPositionNotification()
          => NewPositionNotification?.Invoke(this, new BallMovedEventArgs(Id, X, Y));
    }
}