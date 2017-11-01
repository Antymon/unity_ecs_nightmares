using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Entitas;

public class CollisionSystemTestSuite : EntitasTestSuite {

    [Test]
    public void EntitiesCollide_HealthIsDecremented()
    {
        var player = CreateGameEntity();
        var bullet = CreateGameEntity();

        player.ReplaceHealth(5, 5);
        bullet.ReplaceDamage(1);

        Assert.AreEqual(5,player.health.healthPoints);

        CreateInputEntity().ReplaceCollision(player, bullet);

        UpdateSystems();

        Assert.AreEqual(4,player.health.healthPoints);
    }

    [Test]
    public void EntityLosesHealth_EntityIsDestroyed()
    {
        var player = CreateGameEntity();
        var bullet = CreateGameEntity();

        player.ReplaceHealth(1, 1);
        bullet.ReplaceDamage(1);

        Assert.IsTrue(player.isEnabled);

        CreateInputEntity().ReplaceCollision(player, bullet);

        UpdateSystems();
        UpdateSystems();

        Assert.IsFalse(player.isEnabled);
    }


}
