//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.Data.Test
{
    [TestClass]
    public sealed class DataLayerUnitTest
    {
        /* ======================================================================
           1.  CreateBalls zwraca żądaną liczbę obiektów IBall
           ====================================================================== */
        [TestMethod]
        public void CreateBalls_ReturnsRequestedCount()
        {
            var data = DataAbstractAPI.Create(100, 100);
            var balls = data.CreateBalls(5, radius: 10);

            Assert.AreEqual(5, balls.Count);
        }

        /* ======================================================================
           2.  Współrzędne kul mieszczą się w granicach planszy
           ====================================================================== */
        [TestMethod]
        public void Balls_AreInsideBoard()
        {
            const double W = 200, H = 150, R = 5;

            var data = DataAbstractAPI.Create(W, H);
            var balls = data.CreateBalls(50, R);

            foreach (var b in balls)
            {
                Assert.IsTrue(b.X >= R && b.X <= W - R, "Ball outside X range");
                Assert.IsTrue(b.Y >= R && b.Y <= H - R, "Ball outside Y range");
            }
        }
    }
}
