//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class BallEntity {

    public BallChangedDirectionListenerComponent ballChangedDirectionListener { get { return (BallChangedDirectionListenerComponent)GetComponent(BallComponentsLookup.BallChangedDirectionListener); } }
    public bool hasBallChangedDirectionListener { get { return HasComponent(BallComponentsLookup.BallChangedDirectionListener); } }

    public void AddBallChangedDirectionListener(BallChangedDirectionListener newListener) {
        var index = BallComponentsLookup.BallChangedDirectionListener;
        var component = CreateComponent<BallChangedDirectionListenerComponent>(index);
        component.listener = newListener;
        AddComponent(index, component);
    }

    public void ReplaceBallChangedDirectionListener(BallChangedDirectionListener newListener) {
        var index = BallComponentsLookup.BallChangedDirectionListener;
        var component = CreateComponent<BallChangedDirectionListenerComponent>(index);
        component.listener = newListener;
        ReplaceComponent(index, component);
    }

    public void RemoveBallChangedDirectionListener() {
        RemoveComponent(BallComponentsLookup.BallChangedDirectionListener);
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
public partial class BallEntity : IBallChangedDirectionListener { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class BallMatcher {

    static Entitas.IMatcher<BallEntity> _matcherBallChangedDirectionListener;

    public static Entitas.IMatcher<BallEntity> BallChangedDirectionListener {
        get {
            if (_matcherBallChangedDirectionListener == null) {
                var matcher = (Entitas.Matcher<BallEntity>)Entitas.Matcher<BallEntity>.AllOf(BallComponentsLookup.BallChangedDirectionListener);
                matcher.componentNames = BallComponentsLookup.componentNames;
                _matcherBallChangedDirectionListener = matcher;
            }

            return _matcherBallChangedDirectionListener;
        }
    }
}
