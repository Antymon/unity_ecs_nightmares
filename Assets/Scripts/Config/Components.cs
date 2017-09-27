using System.Collections.Generic;
using UnityEngine;

using Entitas;
using Entitas.CodeGeneration.Attributes;

public class EntityBinding : IComponent
{
    public EntityPrefabNameBinding entitasBinding;
}

public class MarkedToPostponedDestroyComponent : IComponent
{

}

public class PositionComponent : IComponent 
{
    public Vector3 position;
}

public class MovementDirectionComponent : IComponent
{
    public Vector3 direction;
    public bool onlyRotationAffected;
}

public interface IMovementDirectionChangedListener
{
    void OnMovementDirectionChanged(Vector3 direction);
    void OnOrientationChanged(Vector3 direction);
}

public class MovementDirectionChangedListenerComponent : IComponent
{
    public IMovementDirectionChangedListener listener;
}

public class MovementDestinationComponent : IComponent
{
    public Vector3 destination;
    public Vector3 orientation;
}

public interface IMovementDestinationChangedListener
{
    void OnMovementDestinationChanged(Vector3 destination);
    void OnOrientationDestinationChanged(Vector3 destination);
}

public class MovementDestinationChangedListenerComponent : IComponent
{
    public IMovementDestinationChangedListener listener;
}

public class AgentDeadComponent : IComponent
{
    public GameEntity agent;
}

public class AgentComponent : IComponent
{
    public int id;
    public string name;
    public List<IEffect> effects;
}

public class Player : IComponent
{

}

public class EnemyComponent : IComponent
{
    public GameEntity target;
}

public class AIPerceptionComponent : IComponent
{
    public Vector3[] stationaryPositions;
    public int attackDistance;
    public float attackRecoverHealthThreshold;
    public IPositionVerificationCallback callback;
}

public interface IPositionVerificationCallback
{
    bool IsPositionSafe(Vector3 position);
}

public interface IEffect
{
    void Apply(GameEntity entity);
    int GetRepeatInterval();
    bool IsContintous();
}

//for boosters
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
    public GameEntity self;
    public GameEntity other;
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

public class GunComponent : IComponent
{
    public int cooldownTicks;
    public int currentHeat;
    public IShootListener shootListener;
    public bool triggerDown;
    public int range;
    public int damagePerShot;
}

public interface IShootListener
{
    void OnShoot(GameEntity bullet);
}

public class ProjectileComponent : IComponent
{
}

[Unique]
public class LevelComponent : IComponent
{
    public int numberRounds;
    public int effectsAtTimeCap;
    public int currentRound;
    public int roundTime; //sec
    public int roundScoreReward;
}


public class GameOverComponent : IComponent{}
public class GameStartComponent : IComponent{}
public class GamePausedComponent : IComponent{}
public class RoundStartedComponent : IComponent{}
public class RoundFinishedComponent : IComponent{}

[Unique]
public class ScoreComponent : IComponent
{
    public int currentScore;
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
    void OnJoypadMoved(Vector2 direction);
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