//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public MovementDirectionChangedListenerComponent movementDirectionChangedListener { get { return (MovementDirectionChangedListenerComponent)GetComponent(GameComponentsLookup.MovementDirectionChangedListener); } }
    public bool hasMovementDirectionChangedListener { get { return HasComponent(GameComponentsLookup.MovementDirectionChangedListener); } }

    public void AddMovementDirectionChangedListener(IMovementDirectionChangedListener newListener) {
        var index = GameComponentsLookup.MovementDirectionChangedListener;
        var component = CreateComponent<MovementDirectionChangedListenerComponent>(index);
        component.listener = newListener;
        AddComponent(index, component);
    }

    public void ReplaceMovementDirectionChangedListener(IMovementDirectionChangedListener newListener) {
        var index = GameComponentsLookup.MovementDirectionChangedListener;
        var component = CreateComponent<MovementDirectionChangedListenerComponent>(index);
        component.listener = newListener;
        ReplaceComponent(index, component);
    }

    public void RemoveMovementDirectionChangedListener() {
        RemoveComponent(GameComponentsLookup.MovementDirectionChangedListener);
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

    static Entitas.IMatcher<GameEntity> _matcherMovementDirectionChangedListener;

    public static Entitas.IMatcher<GameEntity> MovementDirectionChangedListener {
        get {
            if (_matcherMovementDirectionChangedListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MovementDirectionChangedListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMovementDirectionChangedListener = matcher;
            }

            return _matcherMovementDirectionChangedListener;
        }
    }
}