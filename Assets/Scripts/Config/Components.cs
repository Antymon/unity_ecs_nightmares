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
public class PositionChangedComponent : IComponent
{
    public ulong lastChangeTick;
    public ulong ticksStationary; //how many ticks in 'same' position
    public Vector3 lastPositon;
}

public class MovementStopComponent : IComponent
{

}

//describes movement by direction
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

//describes movement by desired destination
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
    public GameEntity target;
}

public class Player : IComponent
{

}

public class EnemyComponent : IComponent
{
}

public class AIPerceptionComponent : IComponent
{
    public Vector3[] stationaryPositions;
    public int attackDistance;
    //below normalized value of health enemy will seek to recover
    //above will attack
    public float attackRecoverHealthThreshold; 
    public IPositionVerificationCallback callback;
}

public interface IPositionVerificationCallback
{
    bool IsPositionSafe(Vector3 position, GameEntity forAgent);
}

//for pickups
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

[Input]
public class TriggerComponent : IComponent
{
    public GameEntity self;
    public GameEntity other;
    public bool onEnter;
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
public class MatchComponent : IComponent
{
    public int numberRounds;
    public int effectsAtTimeCap;
    public int roundTime; //sec
    public int roundScoreReward;
    public IRandom random;
}

[Unique]
public class PauseComponent : IComponent{}

[Unique]
public class RoundComponent : IComponent
{
    public int currentRound;
}

[Unique]
public class ScoresComponent : IComponent
{
    public Dictionary<int, int> agentIdToScoreMapping;
}

public class GameOverComponent : IComponent{}
public class GameStartComponent : IComponent{}
public class GameRestartComponent : IComponent {}
public class RoundStartedComponent : IComponent{}
public class RoundFinishedComponent : IComponent{}
public class RoundRestartComponent : IComponent {}

[Unique]
[Input]
public class TickComponent : IComponent 
{
    public ulong currentTick; //time measure
}

public class JoypadComponent : IComponent
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

public class RoundCounterComponent : IComponent
{
    public int round;
}

public class HealthBarComponent : IComponent
{
    public int agentId;
    public IHealthBarListener listener;
}

public interface IHealthBarListener
{
    void OnHealthChanged(float value);
    void OnNameChanged(string name);
    void OnScoreChanged(int value);
}

public class GameOverScreenComponent : IComponent
{
    public IGameOverScreenListener listener;
}

public interface IGameOverScreenListener
{
    void OnShow(string message);
}