using TP.ConcurrentProgramming.Data;

[TestClass]
public class BallConstructorUnitTest
{
    [TestMethod]
    public void Constructor_SetsInitialCoordinates()
    {
        double x = 1.23, y = 4.56;
        var ball = new StubBall(x, y, radius: 10);

        Assert.AreEqual(x, ball.X);
        Assert.AreEqual(y, ball.Y);
    }

    private sealed class StubBall : IBall
    {
        public StubBall(double x, double y, double radius)
        { Id = Guid.NewGuid(); X = x; Y = y; Radius = radius; }

        public Guid Id { get; }
        public double X { get; }
        public double Y { get; }
        public double Radius { get; }

        public event EventHandler<BallMovedEventArgs>? NewPositionNotification;
    }
}
