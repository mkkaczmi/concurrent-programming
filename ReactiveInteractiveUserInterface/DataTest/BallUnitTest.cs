//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.Data.Test
{
    /// <summary>
    /// Testy jednostkowe warstwy Data – wyłącznie przez abstrakcyjne API (IBall, DataAbstractAPI).
    /// </summary>
    [TestClass]
    public sealed class BallUnitTest
    {
        /* =========================================================================
           1.  DataAbstractAPI.CreateBalls  ── poprawnie tworzy wskazaną liczbę kul
           ========================================================================= */
        [TestMethod]
        public void CreateBalls_ReturnsRequestedCount()
        {
            DataAbstractAPI data = DataAbstractAPI.Create(100, 100);
            IReadOnlyList<IBall> balls = data.CreateBalls(3, radius: 5);

            Assert.AreEqual(3, balls.Count);
            foreach (var b in balls)
            {
                Assert.IsTrue(b.X >= b.Radius && b.X <= 100 - b.Radius);
                Assert.IsTrue(b.Y >= b.Radius && b.Y <= 100 - b.Radius);
            }
        }

        /* =========================================================================
           2.  IBall.Move(dx,dy)  ── zdarzenie BallMovedEventArgs jest podnoszone
           ========================================================================= */
        [TestMethod]
        public void Move_RaisesNotification()
        {
            var ball = new StubBall(10);
            int raised = 0;

            ball.NewPositionNotification += (_, e) =>
            {
                raised++;
                Assert.AreEqual(ball.Id, e.Id);
                Assert.AreEqual(ball.X, e.X);
                Assert.AreEqual(ball.Y, e.Y);
            };

            ball.Move(2.0, 3.0);

            Assert.AreEqual(1, raised);
            Assert.AreEqual(2.0, ball.X);
            Assert.AreEqual(3.0, ball.Y);
        }

        /* ----------------------------------------------------------------------- *
         *  Lokalna implementacja IBall – używana tylko w tym teście (brak Vector) *
         * ----------------------------------------------------------------------- */
        private sealed class StubBall : IBall
        {
            public StubBall(double radius)
            {
                Id = Guid.NewGuid();
                Radius = radius;
            }

            public Guid Id { get; }
            public double X { get; private set; }
            public double Y { get; private set; }
            public double Radius { get; }

            public event EventHandler<BallMovedEventArgs>? NewPositionNotification;

            internal void Move(double dx, double dy)
            {
                X += dx;
                Y += dy;
                NewPositionNotification?.Invoke(this, new BallMovedEventArgs(Id, X, Y));
            }
        }
    }
}
