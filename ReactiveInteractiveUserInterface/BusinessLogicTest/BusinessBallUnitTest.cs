//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
  [TestClass]
  public class BallUnitTest
  {
    [TestMethod]
    public void MoveTestMethod()
    {
      DataBallFixture dataBallFixture = new DataBallFixture();
      Ball newInstance = new(dataBallFixture);
      int numberOfCallBackCalled = 0;
      newInstance.NewPositionNotification += (sender, position) => { Assert.IsNotNull(sender); Assert.IsNotNull(position); numberOfCallBackCalled++; };
      dataBallFixture.Move();
      Assert.AreEqual<int>(1, numberOfCallBackCalled);
    }

        #region testing instrumentation

        private sealed class DataBallFixture : Data.IBall
        {
            public DataBallFixture(double x = 0, double y = 0, double r = 10)
            {
                Id = Guid.NewGuid();
                X = x;
                Y = y;
                Radius = r;
            }

            /* ----------  IBall ---------- */
            public Guid Id { get; }
            public double X { get; private set; }
            public double Y { get; private set; }
            public double Radius { get; }

            public event EventHandler<Data.BallMovedEventArgs>? NewPositionNotification;

            /* ----------  helpers used in tests ---------- */
            internal void Move(double dx, double dy)
            {
                X += dx;
                Y += dy;
                NewPositionNotification?.Invoke(
                    this,
                    new Data.BallMovedEventArgs(Id, X, Y));
            }
        }
    #endregion testing instrumentation
  }
}