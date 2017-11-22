using Entitas;
using NUnit.Framework;

public class JoypadSystemTestSuite : EntitasTestSuite
{
    [Test]
    public void JoypadDisabled_OnPlayerAndTouchAppears_JoypadEnabled()
    {
        Assert.Fail("Not implemented");
    }

    [Test]
    public void JoypadEnabled_NoTouchEventsButPlayerExists_JoypadDisabled()
    {
        Assert.Fail("Not implemented");
    }

    [Test]
    public void JoypadEnabled_TouchEventsButNoPlayerExists_JoypadDisabled()
    {
        Assert.Fail("Not implemented");
    }

    [Test]
    public void JoypadEnabled_OnTouchMoved_JoypadMoves()
    {
        Assert.Fail("Not implemented");
    }

    [Test]
    public void JoypadEnabled_MoreTouches_JoypadKeepsOriginalTouchId()
    {
        Assert.Fail("Not implemented");
    }
}

