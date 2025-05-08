//____________________________________________________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//_____________________________________________________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TP.ConcurrentProgramming.BusinessLogic;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
    [TestClass]
    public sealed class BallLogicUnitTests
    {
        /* ======================================================================
           1.  Fabryka  ── nie zwraca null‑a
           ====================================================================== */
        [TestMethod]
        public void Create_ReturnsInstance()
        {
            var data = new StubDataLayer();
            var logic = BusinessLogicAbstractAPI.Create(data);

            Assert.IsNotNull(logic);
        }

        /* ======================================================================
           2.  StartSimulation  ── wywołuje CreateBalls na warstwie Data
           ====================================================================== */
        [TestMethod]
        public void StartSimulation_CreatesRequestedNumberOfBalls()
        {
            var data = new StubDataLayer();
            var logic = BusinessLogicAbstractAPI.Create(data);

            logic.StartSimulation(7, radius: 10);

            Assert.AreEqual(7, data.CreatedCount);
        }

        /* ======================================================================
           3.  BallMoved event  ── jest podnoszone przynajmniej raz
           ====================================================================== */
        [TestMethod]
        public void Tick_RaisesBallMovedEvent()
        {
            var data = new StubDataLayer();
            data.CreateBalls(1, 10);             // wstępna kula

            var logic = BusinessLogicAbstractAPI.Create(data);
            int raised = 0;

            logic.BallMoved += (_, _) => raised++;

            logic.StartSimulation(0);            // uruchamiamy timer
            System.Threading.Thread.Sleep(150);  // ~3 ticki (50 ms)
            logic.StopSimulation();

            Assert.IsTrue(raised > 0);
        }

        /* ======================================================================
           STUB WARSTWY DANYCH  --------------------------------------------------
           ====================================================================== */
        private sealed class StubDataLayer : DataAbstractAPI
        {
            public int CreatedCount { get; private set; }

            public override IReadOnlyList<IBall> CreateBalls(int count, double radius)
            {
                CreatedCount += count;
                for (int i = 0; i < count; i++)
                    _balls.Add(new StubBall(radius));
                return _balls.AsReadOnly();
            }

            public override IReadOnlyList<IBall> GetBalls() => _balls.AsReadOnly();

            private readonly List<IBall> _balls = new();
        }

        /* --------------------------------------------------------------------- */
        private sealed class StubBall : IBall
        {
            public StubBall(double radius)
            {
                Id = Guid.NewGuid();
                Radius = radius;
            }

            public Guid Id { get; }
            public double X { get; } = 0;
            public double Y { get; } = 0;
            public double Radius { get; }

            public event EventHandler<BallMovedEventArgs>? NewPositionNotification;
        }
    }
}
