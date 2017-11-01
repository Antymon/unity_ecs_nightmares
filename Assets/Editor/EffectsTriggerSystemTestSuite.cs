using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EffectsTriggerSystemTestSuite : EntitasTestSuite, IMovementDestinationChangedListener
{
    private GameEntity CreateHealthEffectEntity()
    {
        var effectEntity = CreateGameEntity();
        var effect = new AddHealthEffect();
        effect.healthPoints = 2;
        effectEntity.ReplaceEffect(effect);

        return effectEntity;
    }

    private GameEntity CreateExclusiveEffectEntity()
    {
        var effectEntity = CreateGameEntity();
        var effect = new MovementInverterEffect();
        effect.lastingTicks = ulong.MaxValue;
        effectEntity.ReplaceEffect(effect);

        return effectEntity;
    }


    [Test]
    public void CollectibleEffectEntity_IsDiscardedAfterUsing()
    {
        var player = CreateGameEntity();
        player.ReplaceAgent(1, "player", new List<IEffect>(), newTarget: null);
        player.ReplaceHealth(3, 5);

        var effectEntity = CreateHealthEffectEntity();

        Assert.IsEmpty(player.agent.effects);

        CreateInputEntity().ReplaceTrigger(player, effectEntity,newOnEnter: true);

        UpdateSystems();

        Assert.IsTrue(player.agent.effects.Contains(effectEntity.effect.effect));

        UpdateSystems();

        Assert.IsFalse(effectEntity.isEnabled);
    }

    [Test]
    public void EffectIsNotApplicable_NothingHappens()
    {
        //without health compoinent on entity; health effects are not applicable
        var player = CreateGameEntity();
        player.ReplaceAgent(1, "player", new List<IEffect>(), newTarget: null);

        var effectEntity = CreateHealthEffectEntity();

        Assert.IsEmpty(player.agent.effects);

        CreateInputEntity().ReplaceTrigger(player, effectEntity, newOnEnter: true);

        UpdateSystems();

        //still empty since trigger was discarded
        Assert.IsEmpty(player.agent.effects);
    }

    [Test]
    public void ExclusiveEffectOfSameType_IsNotQueuedMoreThanOnce()
    {
        var enemy = CreateGameEntity();

        //just satisfying interface with fake
        enemy.AddMovementDestinationChangedListener(this);

        var player = CreateGameEntity();
        player.ReplaceAgent(1, "player", new List<IEffect>(), newTarget: enemy);
        player.ReplaceHealth(3, 5);

        var effectEntity = CreateExclusiveEffectEntity();

        Assert.IsEmpty(player.agent.effects);

        CreateInputEntity().ReplaceTrigger(player, effectEntity, newOnEnter: true);

        UpdateSystems();

        Assert.IsTrue(player.agent.effects.Contains(effectEntity.effect.effect));

        CreateInputEntity().ReplaceTrigger(player, CreateExclusiveEffectEntity(), newOnEnter: true);

        UpdateSystems();

        //still one despite attempt to add one more
        Assert.AreEqual(1, player.agent.effects.Count);
    }

    [Test]
    public void NonExclusiveEffectOfSameType_CanBeQueuedMoreThanOnce()
    {
        var player = CreateGameEntity();
        player.ReplaceAgent(1, "player", new List<IEffect>(), newTarget: null);
        player.ReplaceHealth(3, 5);

        var effectEntity = CreateHealthEffectEntity();

        Assert.IsEmpty(player.agent.effects);

        CreateInputEntity().ReplaceTrigger(player, effectEntity, newOnEnter: true);

        UpdateSystems();

        Assert.IsTrue(player.agent.effects.Contains(effectEntity.effect.effect));

        CreateInputEntity().ReplaceTrigger(player, CreateHealthEffectEntity(), newOnEnter: true);

        UpdateSystems();

        Assert.AreEqual(2, player.agent.effects.Count);
    }

    [Test]
    public void EffectIsNotRelevant_GetsRemovedFromQueue()
    {
        var player = CreateGameEntity();
        player.ReplaceAgent(1, "player", new List<IEffect>(), newTarget: null);
        player.ReplaceHealth(3, 5);

        var effectEntity = CreateHealthEffectEntity();
        var effect = effectEntity.effect.effect;

        Assert.IsEmpty(player.agent.effects);

        CreateInputEntity().ReplaceTrigger(player, effectEntity, newOnEnter: true);

        UpdateSystems();

        Assert.IsTrue(player.agent.effects.Contains(effect));

        CreateInputEntity().ReplaceTrigger(player, CreateHealthEffectEntity(), newOnEnter: true);

        UpdateSystems();

        Assert.AreEqual(2, player.agent.effects.Count);

        effectEntity = CreateHealthEffectEntity();
        effectEntity.ReplaceEffect(effect);

        CreateInputEntity().ReplaceTrigger(player, effectEntity, newOnEnter: false);

        UpdateSystems();

        Assert.AreEqual(1, player.agent.effects.Count);
        Assert.IsFalse(player.agent.effects.Contains(effect));
    }

    public void OnMovementDestinationChanged(Vector3 destination)
    {
        throw new System.NotImplementedException();
    }

    public void OnOrientationDestinationChanged(Vector3 destination)
    {
        throw new System.NotImplementedException();
    }
}
