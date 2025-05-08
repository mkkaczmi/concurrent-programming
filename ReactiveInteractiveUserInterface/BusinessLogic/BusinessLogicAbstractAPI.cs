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
    /// Abstrakcyjny kontrakt warstwy logiki – bez IDisposable i starego wektora.
    /// </summary>
    public abstract class BusinessLogicAbstractAPI
    {
        /* ----------  FABRYKA  ---------- */
        public static BusinessLogicAbstractAPI Create(DataAbstractAPI? data = null)
          => new BusinessLogicImplementation(data);

        /* ----------  PUBLIC API  ---------- */

        /// <summary>Tworzy <paramref name="balls"/> kul i uruchamia symulację.</summary>
        public abstract void StartSimulation(int balls, double radius = 10);

        /// <summary>Zatrzymuje timer symulacji.</summary>
        public abstract void StopSimulation();

        /// <summary>Zdarzenie publikowane po każdym przesunięciu kuli.</summary>
        public abstract event EventHandler<BallMovedEventArgs>? BallMoved;

        /* ----------  DANE POMOCNICZE  ---------- */

        public static readonly Dimensions TableDefaults = new(10.0, 600.0, 400.0);
    }

    /* ========================================================================= */

    /// <summary>Immutable record z domyślnymi wymiarami stołu i kul.</summary>
    public sealed record Dimensions(double BallRadius, double TableWidth, double TableHeight);
}
