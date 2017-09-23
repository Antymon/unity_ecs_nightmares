using Assets.Scripts.Components;
using Entitas;
using UnityEngine;


[Ball,Pad,Block]
public sealed class PositionComponent : IComponent {

    public float x;
    public float y;
}

[Background]
public class TriggerComponent : IComponent
{
    public Entity other;
}

[Pad]
public class PlayerComponent : IComponent
{
    public int id;
}

[Pad]
public class KeyInputComponent : IComponent
{
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
}

[Ball, Pad]
public sealed class MovableComponent : IComponent
{
    
}

[Input,Pad, Block]
public class CollisionComponent : IComponent
{
    public Collision2D collision;
    public Vector2 velocity;
    public Entity self;
    public Entity other;
}

[Ball]
public class ProcessedCollisionComponent : IComponent
{
    public ICollisionInfo collision;
}

public interface BallChangedDirectionListener
{
    void DirectionChanged(Vector2 direction);
}

[Ball,Pad]
public class BallChangedDirectionListenerComponent : IComponent
{
    public BallChangedDirectionListener listener;
}

[Ball,Pad]
public class PositionTrackerComponent : IComponent
{
    public SinkingQueue<Vector2> trackedPositions;
}

[Input]
public class KeyPressedComponent : IComponent
{
    public KeyCode keyCode;
}

[Ball]
public class BallOutComponent : IComponent
{

}