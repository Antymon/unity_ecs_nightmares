//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class BlockEntity {

    public CollisionComponent collision { get { return (CollisionComponent)GetComponent(BlockComponentsLookup.Collision); } }
    public bool hasCollision { get { return HasComponent(BlockComponentsLookup.Collision); } }

    public void AddCollision(UnityEngine.Collision2D newCollision, UnityEngine.Vector2 newVelocity, Entitas.Entity newSelf, Entitas.Entity newOther) {
        var index = BlockComponentsLookup.Collision;
        var component = CreateComponent<CollisionComponent>(index);
        component.collision = newCollision;
        component.velocity = newVelocity;
        component.self = newSelf;
        component.other = newOther;
        AddComponent(index, component);
    }

    public void ReplaceCollision(UnityEngine.Collision2D newCollision, UnityEngine.Vector2 newVelocity, Entitas.Entity newSelf, Entitas.Entity newOther) {
        var index = BlockComponentsLookup.Collision;
        var component = CreateComponent<CollisionComponent>(index);
        component.collision = newCollision;
        component.velocity = newVelocity;
        component.self = newSelf;
        component.other = newOther;
        ReplaceComponent(index, component);
    }

    public void RemoveCollision() {
        RemoveComponent(BlockComponentsLookup.Collision);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class BlockEntity : ICollision { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class BlockMatcher {

    static Entitas.IMatcher<BlockEntity> _matcherCollision;

    public static Entitas.IMatcher<BlockEntity> Collision {
        get {
            if (_matcherCollision == null) {
                var matcher = (Entitas.Matcher<BlockEntity>)Entitas.Matcher<BlockEntity>.AllOf(BlockComponentsLookup.Collision);
                matcher.componentNames = BlockComponentsLookup.componentNames;
                _matcherCollision = matcher;
            }

            return _matcherCollision;
        }
    }
}
