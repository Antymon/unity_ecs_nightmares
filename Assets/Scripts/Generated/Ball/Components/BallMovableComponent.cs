//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class BallEntity {

    static readonly MovableComponent movableComponent = new MovableComponent();

    public bool isMovable {
        get { return HasComponent(BallComponentsLookup.Movable); }
        set {
            if (value != isMovable) {
                if (value) {
                    AddComponent(BallComponentsLookup.Movable, movableComponent);
                } else {
                    RemoveComponent(BallComponentsLookup.Movable);
                }
            }
        }
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
public partial class BallEntity : IMovable { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class BallMatcher {

    static Entitas.IMatcher<BallEntity> _matcherMovable;

    public static Entitas.IMatcher<BallEntity> Movable {
        get {
            if (_matcherMovable == null) {
                var matcher = (Entitas.Matcher<BallEntity>)Entitas.Matcher<BallEntity>.AllOf(BallComponentsLookup.Movable);
                matcher.componentNames = BallComponentsLookup.componentNames;
                _matcherMovable = matcher;
            }

            return _matcherMovable;
        }
    }
}
