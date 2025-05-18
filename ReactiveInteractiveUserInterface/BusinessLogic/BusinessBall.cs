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
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class Ball : IBall
  {
    private readonly Data.IBall dataBall;
    private IPosition currentPosition;
    private bool disposed = false;

    public Ball(Data.IBall ball)
    {
      this.dataBall = ball;
      this.dataBall.NewPositionNotification += RaisePositionChangeEvent;
      this.currentPosition = new Position(0, 0);
    }

    #region IBall

    public event EventHandler<IPosition>? NewPositionNotification;

    public IPosition CurrentPosition => currentPosition;

    public double Diameter => BusinessLogicAbstractAPI.GetDimensions.BallDimension;

    public void UpdatePosition(IPosition newPosition)
    {
      if (disposed)
        throw new ObjectDisposedException(nameof(Ball));

      currentPosition = newPosition;
      NewPositionNotification?.Invoke(this, newPosition);
    }

    public void Dispose()
    {
      if (!disposed)
      {
        dataBall.NewPositionNotification -= RaisePositionChangeEvent;
        disposed = true;
      }
    }

    #endregion IBall

    #region private

    private void RaisePositionChangeEvent(object? sender, Data.IVector e)
    {
      if (!disposed)
      {
        UpdatePosition(new Position(e.x, e.y));
      }
    }

    #endregion private
  }
}