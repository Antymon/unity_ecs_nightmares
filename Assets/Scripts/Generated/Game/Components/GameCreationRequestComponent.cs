//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly CreationRequestComponent creationRequestComponent = new CreationRequestComponent();

    public bool isCreationRequest {
        get { return HasComponent(GameComponentsLookup.CreationRequest); }
        set {
            if (value != isCreationRequest) {
                if (value) {
                    AddComponent(GameComponentsLookup.CreationRequest, creationRequestComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.CreationRequest);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherCreationRequest;

    public static Entitas.IMatcher<GameEntity> CreationRequest {
        get {
            if (_matcherCreationRequest == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CreationRequest);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCreationRequest = matcher;
            }

            return _matcherCreationRequest;
        }
    }
}