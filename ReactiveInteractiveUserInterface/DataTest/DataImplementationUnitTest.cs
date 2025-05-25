//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]
  public class DataImplementationUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      using (DataImplementation newInstance = new DataImplementation())
      {
        IEnumerable<IBall>? ballsList = null;
        newInstance.CheckBallsList(x => ballsList = x);
        Assert.IsNotNull(ballsList);
        int numberOfBalls = 0;
        newInstance.CheckNumberOfBalls(x => numberOfBalls = x);
        Assert.AreEqual<int>(0, numberOfBalls);
      }
    }

    [TestMethod]
    public void DisposeTestMethod()
    {
      DataImplementation newInstance = new DataImplementation();
      bool newInstanceDisposed = false;
      newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
      Assert.IsFalse(newInstanceDisposed);
      newInstance.Dispose();
      newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
      Assert.IsTrue(newInstanceDisposed);
      IEnumerable<IBall>? ballsList = null;
      newInstance.CheckBallsList(x => ballsList = x);
      Assert.IsNotNull(ballsList);
      newInstance.CheckNumberOfBalls(x => Assert.AreEqual<int>(0, x));
      Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Dispose());
      Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Start(0, (position, ball) => { }));
    }

    [TestMethod]
    public void StartTestMethod()
    {
      using (DataImplementation newInstance = new DataImplementation())
      {
        int numberOfCallbackInvoked = 0;
        int numberOfBalls2Create = 10;
        newInstance.Start(
          numberOfBalls2Create,
          (startingPosition, ball) =>
          {
            numberOfCallbackInvoked++;
            Assert.IsTrue(startingPosition.x >= 0);
            Assert.IsTrue(startingPosition.y >= 0);
            Assert.IsNotNull(ball);
          });
        Assert.AreEqual<int>(numberOfBalls2Create, numberOfCallbackInvoked);
        newInstance.CheckNumberOfBalls(x => Assert.AreEqual<int>(10, x));
      }
    }

    [TestMethod]
    public void ElasticCollisionTest()
    {
      using (DataImplementation newInstance = new DataImplementation())
      {
        // Create two balls with known positions and velocities
        Vector pos1 = new Vector(100, 100);
        Vector pos2 = new Vector(120, 100); // 20 units apart (ball diameter)
        Vector vel1 = new Vector(5, 0); // Moving right
        Vector vel2 = new Vector(-5, 0); // Moving left

        Ball ball1 = new Ball(pos1, vel1);
        Ball ball2 = new Ball(pos2, vel2);
        ball1.Mass = 1.0;
        ball2.Mass = 1.0;

        newInstance.AddBall(ball1);
        newInstance.AddBall(ball2);

        // Simulate one frame of movement
        newInstance.SimulateOneFrame();

        // After elastic collision, balls should exchange velocities
        Assert.AreEqual(-5, ball1.Velocity.x, 0.001);
        Assert.AreEqual(0, ball1.Velocity.y, 0.001);
        Assert.AreEqual(5, ball2.Velocity.x, 0.001);
        Assert.AreEqual(0, ball2.Velocity.y, 0.001);
      }
    }

    [TestMethod]
    public void MassConsiderationTest()
    {
      using (DataImplementation newInstance = new DataImplementation())
      {
        // Create two balls with different masses
        Vector pos1 = new Vector(100, 100);
        Vector pos2 = new Vector(120, 100);
        Vector vel1 = new Vector(5, 0);
        Vector vel2 = new Vector(-5, 0);

        Ball ball1 = new Ball(pos1, vel1);
        Ball ball2 = new Ball(pos2, vel2);
        ball1.Mass = 1.0;
        ball2.Mass = 2.0; // Twice the mass

        newInstance.AddBall(ball1);
        newInstance.AddBall(ball2);

        // Simulate one frame of movement
        newInstance.SimulateOneFrame();

        // After collision, velocities should be affected by mass
        // Lighter ball should bounce back faster
        Assert.IsTrue(Math.Abs(ball1.Velocity.x) > Math.Abs(ball2.Velocity.x));
      }
    }

    [TestMethod]
    public void WallCollisionTest()
    {
      using (DataImplementation newInstance = new DataImplementation())
      {
        // Create a ball moving towards the right wall
        Vector pos = new Vector(380, 100); // Near right wall
        Vector vel = new Vector(5, 0); // Moving right
        Ball ball = new Ball(pos, vel);
        ball.Mass = 1.0;

        newInstance.AddBall(ball);
        newInstance.UpdateBoxDimensions(400, 400);

        // Simulate one frame of movement
        newInstance.SimulateOneFrame();

        // Ball should bounce off the wall
        Assert.IsTrue(ball.Velocity.x < 0);
        Assert.AreEqual(0, ball.Velocity.y, 0.001);
      }
    }
  }
}