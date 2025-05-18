//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel.MVVMLight;
using ModelIBall = TP.ConcurrentProgramming.Presentation.Model.IBall;

namespace TP.ConcurrentProgramming.Presentation.ViewModel
{
  public class MainWindowViewModel : IDisposable
  {
    #region ctor

    public MainWindowViewModel() : this(null)
    {
    }

    public MainWindowViewModel(ModelAbstractApi? modelLayerAPI)
    {
      ModelLayer = modelLayerAPI ?? ModelAbstractApi.CreateModel();
      Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x));
      StartCommand = new RelayCommand<string>(ExecuteStart);
      StopCommand = new RelayCommand(ExecuteStop);
    }

    #endregion ctor

    #region Commands

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    private void ExecuteStart(string ballCountText)
    {
      if (int.TryParse(ballCountText, out int numberOfBalls) && numberOfBalls > 0)
      {
        Start(numberOfBalls);
      }
      else
      {
        System.Windows.MessageBox.Show("Please enter a valid number of balls (greater than 0).", "Invalid Input", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
      }
    }

    private void ExecuteStop()
    {
      Stop();
    }

    #endregion Commands

    #region public API

    public void Start(int numberOfBalls)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(MainWindowViewModel));
      ModelLayer.Start(numberOfBalls);
    }

    public void Stop()
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(MainWindowViewModel));
      ModelLayer.Stop();
      Balls.Clear();
      // Dispose the current observer
      Observer?.Dispose();
      // Create a new observer for the next start
      Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x));
    }

    public ObservableCollection<ModelIBall> Balls { get; } = new ObservableCollection<ModelIBall>();

    #endregion public API

    #region IDisposable

    public void Dispose()
    {
      if (!Disposed)
      {
        Observer?.Dispose();
        ModelLayer.Dispose();
        Disposed = true;
      }
    }

    #endregion IDisposable

    #region private

    private bool Disposed = false;
    private readonly ModelAbstractApi ModelLayer;
    private IDisposable? Observer;

    #endregion private
  }
}