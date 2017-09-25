//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    public CollisionComponent collision { get { return (CollisionComponent)GetComponent(InputComponentsLookup.Collision); } }
    public bool hasCollision { get { return HasComponent(InputComponentsLookup.Collision); } }

    public void AddCollision(GameEntity newSelf, GameEntity newOther) {
        var index = InputComponentsLookup.Collision;
        var component = CreateComponent<CollisionComponent>(index);
        component.self = newSelf;
        component.other = newOther;
        AddComponent(index, component);
    }

    public void ReplaceCollision(GameEntity newSelf, GameEntity newOther) {
        var index = InputComponentsLookup.Collision;
        var component = CreateComponent<CollisionComponent>(index);
        component.self = newSelf;
        component.other = newOther;
        ReplaceComponent(index, component);
    }

    public void RemoveCollision() {
        RemoveComponent(InputComponentsLookup.Collision);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherCollision;

    public static Entitas.IMatcher<InputEntity> Collision {
        get {
            if (_matcherCollision == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.Collision);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherCollision = matcher;
            }

            return _matcherCollision;
        }
    }
}
