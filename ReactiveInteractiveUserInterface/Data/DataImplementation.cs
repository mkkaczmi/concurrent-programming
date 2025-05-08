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
using System.Collections.Generic;

namespace TP.ConcurrentProgramming.Data.Impl        // ❶ warstwa implementacyjna
{
    /// <summary>
    /// Prosta, pamięciowa implementacja warstwy danych – przechowuje listę kul.
    /// Nie ma timera ani logiki ruchu; tę przejmie Business Logic.
    /// </summary>
    internal sealed class DataLayer : DataAbstractAPI
    {
        /* ----------  ctor  ---------- */
        internal DataLayer(double width, double height)
        {
            _width = width;
            _height = height;
        }

        /* ----------  DataAbstractAPI ---------- */
        public override IReadOnlyList<IBall> CreateBalls(int count, double radius)
        {
            for (int i = 0; i < count; i++)
            {
                var x = _rng.NextDouble() * (_width - 2 * radius) + radius;
                var y = _rng.NextDouble() * (_height - 2 * radius) + radius;
                _balls.Add(new Ball(Guid.NewGuid(), x, y, radius));     // ❷ nowy konstruktor
            }
            return _balls.AsReadOnly();
        }

        public override IReadOnlyList<IBall> GetBalls() => _balls.AsReadOnly();

        /* ----------  pola prywatne ---------- */
        private readonly double _width, _height;
        private readonly Random _rng = new();
        private readonly List<IBall> _balls = new();
    }
}
