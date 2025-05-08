using System;

namespace TP.ConcurrentProgramming.Data
{
    /// <summary>
    /// Argument zdarzenia wysyłanego, gdy kula zmieni pozycję.
    /// </summary>
    public sealed class BallMovedEventArgs : EventArgs
    {
        public Guid Id { get; }
        public double X { get; }
        public double Y { get; }

        public BallMovedEventArgs(Guid id, double x, double y)
          => (Id, X, Y) = (id, x, y);
    }
}
