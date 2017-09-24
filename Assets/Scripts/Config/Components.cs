﻿using System.Collections.Generic;
using UnityEngine;

using Entitas;
using Entitas.CodeGeneration.Attributes;

public class EntityBinding : IComponent
{
    public EntityPrefabNameBinding entitasBinding;
}

public class PositionComponent : IComponent {

    public Vector3 position;
}

public class AgentComponent : IComponent
{
    public int id;
    public string name;
    public IEnumerable<IEffect> effects;
}

public class AggressorComponent : IComponent
{
    public Entity target;
}

public interface IEffect
{

}

//for spawner boosters
public class EffectComponent : IComponent
{
    public IEffect effect;
}

[Input]
public class TouchesComponent : IComponent
{
    public Touch[] touches;
}


[Input]
public class CollisionComponent : IComponent
{
    public Entity self;
    public Entity other;
}

public interface HealthChangedListener
{
    void HealthChanged(Entity entity);
}

public class HealthChangedListenerComponent : IComponent
{
    public HealthChangedListener listener;
}

public class HealthComponent : IComponent
{
    public int healthPoints;
    public int healthPointsCap;
}

public class DamageComponent : IComponent
{
    public int healthPointsDamaged;
}

public class SpawnerComponent : IComponent
{
    public int roundCap;
    public float chanceOfSpawning; //normalized
}

public class ProjectileComponent : IComponent
{
    public long cooldownTime; //ticks, time betweent consecutive shots
}

[Unique]
public class LevelComponent : IComponent
{
    public int numberRounds;
    public int spawnersCap;
    public int currentRound;
    public int wonByPlayer;
    public long roundTime; //ticks
}

[Unique]
public class TickComponent : IComponent
{
    public long currentTick;
}

public class HealthBarComponent : IComponent
{
    public int actorId;
}


public class JoystickComponent : IComponent
{
    public bool enabled;
    public int touchId;
}

public class JoypadBindingComponent : IComponent
{
    public float radius;
    public IJoypadMovedListener listener;
}

public interface IJoypadMovedListener
{
    void JoypadMoved(Vector2 direction);
}

[Unique]
public class PauseComponent : IComponent 
{ 

}

[Unique]
public class RoundCounterComponent : IComponent
{
    public int round;
}

[Unique]
public class ScoreCounterComponent : IComponent
{
    public int score;
}

[Unique]
public class HealthDecreaseOverlayComponent : IComponent
{

}