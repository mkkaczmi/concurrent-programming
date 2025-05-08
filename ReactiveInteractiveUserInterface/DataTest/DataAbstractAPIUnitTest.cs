//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//_____________________________________________________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.Data.Test
{
    [TestClass]
    public sealed class DataAbstractAPIUnitTest
    {
        /// <summary>
        /// 1️⃣  Fabryka <see cref="DataAbstractAPI.Create"/> zwraca poprawną instancję
        ///     i kolejne wywołania tworzą oddzielne obiekty.
        /// </summary>
        [TestMethod]
        public void Create_ReturnsIndependentInstances()
        {
            var d1 = DataAbstractAPI.Create(300, 200);
            var d2 = DataAbstractAPI.Create(300, 200);

            Assert.IsNotNull(d1);
            Assert.IsNotNull(d2);
            Assert.AreNotSame(d1, d2);
        }

        /// <summary>
        /// 2️⃣  <see cref="DataAbstractAPI.CreateBalls"/> tworzy dokładnie żądaną liczbę kul.
        /// </summary>
        [TestMethod]
        public void CreateBalls_ReturnsRequestedCount()
        {
            var data = DataAbstractAPI.Create(100, 100);
            var balls = data.CreateBalls(3, radius: 8);

            Assert.AreEqual(3, balls.Count);
        }
    }
}
