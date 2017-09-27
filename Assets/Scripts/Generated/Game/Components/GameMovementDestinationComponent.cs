//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public MovementDestinationComponent movementDestination { get { return (MovementDestinationComponent)GetComponent(GameComponentsLookup.MovementDestination); } }
    public bool hasMovementDestination { get { return HasComponent(GameComponentsLookup.MovementDestination); } }

    public void AddMovementDestination(UnityEngine.Vector3 newDestination, UnityEngine.Vector3 newOrientation) {
        var index = GameComponentsLookup.MovementDestination;
        var component = CreateComponent<MovementDestinationComponent>(index);
        component.destination = newDestination;
        component.orientation = newOrientation;
        AddComponent(index, component);
    }

    public void ReplaceMovementDestination(UnityEngine.Vector3 newDestination, UnityEngine.Vector3 newOrientation) {
        var index = GameComponentsLookup.MovementDestination;
        var component = CreateComponent<MovementDestinationComponent>(index);
        component.destination = newDestination;
        component.orientation = newOrientation;
        ReplaceComponent(index, component);
    }

    public void RemoveMovementDestination() {
        RemoveComponent(GameComponentsLookup.MovementDestination);
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
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMovementDestination;

    public static Entitas.IMatcher<GameEntity> MovementDestination {
        get {
            if (_matcherMovementDestination == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MovementDestination);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMovementDestination = matcher;
            }

            return _matcherMovementDestination;
        }
    }
}