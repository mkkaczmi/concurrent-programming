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

namespace TP.ConcurrentProgramming.Data
{
    /// <summary>
    /// Abstrakcyjny kontrakt warstwy danych.
    /// Przechowuje wyłącznie stan kul – bez logiki ruchu, timerów, IO czy baz danych.
    /// </summary>
    public abstract class DataAbstractAPI
    {
        /* ----------  FABRYKA  ---------- */
        /// <summary>Tworzy domyślną implementację <see cref="DataLayer"/>.</summary>
        public static DataAbstractAPI Create(double boardWidth = 600, double boardHeight = 400)
          => new Impl.DataLayer(boardWidth, boardHeight);

        /* ----------  PUBLIC API  ---------- */
        /// <summary>Generuje <paramref name="count"/> nowych kul o promieniu <paramref name="radius"/>.</summary>
        public abstract IReadOnlyList<IBall> CreateBalls(int count, double radius);

        /// <summary>Zwraca migawkę wszystkich kul aktualnie przechowywanych w warstwie danych.</summary>
        public abstract IReadOnlyList<IBall> GetBalls();
    }

    /* ===================================================================== */

    /// <summary>Językowa reprezentacja kuli – wykorzystywana przez wyższe warstwy.</summary>
    public interface IBall
    {
        Guid Id { get; }
        double X { get; }
        double Y { get; }
        double Radius { get; }

        /// <summary>Zdarzenie wysyłane, gdy kula zmieni pozycję.</summary>
        event EventHandler<BallMovedEventArgs>? NewPositionNotification;
    }
}
