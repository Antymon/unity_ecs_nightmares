using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Entitas;

public class HealthHelpersTestSuite : EntitasTestSuite
{

    [Test]
    public void OnAddHealth_HealthNeverExceedsCap()
    {
        var player = CreateGameEntity();
        player.ReplaceHealth(4,5);

        Assert.AreEqual(4, player.health.healthPoints);

        HealthHelpers.AddHealth(player.health, 10);

        Assert.AreEqual(5, player.health.healthPoints);

    }

    [Test]
    public void OnAddHealth_HealthNeverFallsBelowZero()
    {
        var player = CreateGameEntity();
        player.ReplaceHealth(4, 5);

        Assert.AreEqual(4, player.health.healthPoints);

        HealthHelpers.AddHealth(player.health, -10);

        Assert.AreEqual(0, player.health.healthPoints);
    }

    [Test]
    public void For0Health_NormalizedHealthIs0()
    {
        var player = CreateGameEntity();
        player.ReplaceHealth(0, 5);

        Assert.AreEqual(0, HealthHelpers.GetNormalizedHealth(player.health));
    }

    [Test]
    public void ForMaxHealth_NormalizedHeatlhIs1()
    {
        var player = CreateGameEntity();
        player.ReplaceHealth(5, 5);

        Assert.AreEqual(1, HealthHelpers.GetNormalizedHealth(player.health));
    }

    [Test]
    public void ForHalfMaxHealth_NormalizedHealthIsHalf()
    {
        var player = CreateGameEntity();
        player.ReplaceHealth(3, 6);

        Assert.AreEqual(.5f, HealthHelpers.GetNormalizedHealth(player.health));
    }
}
