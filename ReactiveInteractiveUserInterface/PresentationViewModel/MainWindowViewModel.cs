//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using TP.ConcurrentProgramming.BusinessLogic;

namespace TP.ConcurrentProgramming.Presentation.ViewModel
{
    /// <summary>
    /// Najprostszy ViewModel okna głównego – uruchamia i zatrzymuje symulację kul.
    /// W kolejnych etapach możesz tu dodać właściwości, komendy, kolekcję kul itp.
    /// </summary>
    public sealed class MainWindowViewModel : IDisposable
    {
        /* ----------  konstruktor  ---------- */

        private readonly BusinessLogicAbstractAPI _logic = BusinessLogicAbstractAPI.Create();

        /* ----------  API dla View  ---------- */

        /// <summary>Rozpoczyna symulację z <paramref name="balls"/> kulami.</summary>
        public void Start(int balls, double radius = 10) =>
          _logic.StartSimulation(balls, radius);

        /// <summary>Zatrzymuje timer logiki.</summary>
        public void Stop() =>
          _logic.StopSimulation();

        /* ----------  IDisposable  ---------- */

        public void Dispose() => Stop();
    }
}
