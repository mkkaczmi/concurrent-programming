//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the ‘Watch’ button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    /// <summary>
    /// Warstwa logiki – publikuje zdarzenie <see cref="BallMoved"/> z nowymi
    /// współrzędnymi kul w zadanym interwale czasu.
    /// </summary>
    public sealed class BusinessLogicImplementation : BusinessLogicAbstractAPI
    {
        /* ---------- ctor ---------- */

        public BusinessLogicImplementation(DataAbstractAPI? data = null)
        {
            _data = data ?? DataAbstractAPI.Create();
            _timer = new System.Timers.Timer(50) { AutoReset = true }; // ~20 FPS
            _timer.Elapsed += (_, _) => Tick();
        }

        /* ---------- BusinessLogicAbstractAPI ---------- */

        public override void StartSimulation(int balls, double radius = 10)
        {
            _data.CreateBalls(balls, radius);
            _timer.Start();
        }

        public override void StopSimulation() => _timer.Stop();

        public override event EventHandler<BallMovedEventArgs>? BallMoved;

        /* ---------- wewnętrzne ---------- */

        private void Tick()
        {
            foreach (var b in _data.GetBalls())
            {
                double dx = (_rng.NextDouble() - 0.5) * 10;
                double dy = (_rng.NextDouble() - 0.5) * 10;

                // NIE modyfikujemy obiektu z warstwy danych – publikujemy nowe położenie
                double nx = b.X + dx;
                double ny = b.Y + dy;

                BallMoved?.Invoke(this, new BallMovedEventArgs(b.Id, nx, ny));
            }
        }

        /* ---------- pola ---------- */

        private readonly DataAbstractAPI _data;
        private readonly System.Timers.Timer _timer;
        private readonly Random _rng = new();
    }
}
