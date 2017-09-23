//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ProjectileComponent projectile { get { return (ProjectileComponent)GetComponent(GameComponentsLookup.Projectile); } }
    public bool hasProjectile { get { return HasComponent(GameComponentsLookup.Projectile); } }

    public void AddProjectile(long newCooldownTime) {
        var index = GameComponentsLookup.Projectile;
        var component = CreateComponent<ProjectileComponent>(index);
        component.cooldownTime = newCooldownTime;
        AddComponent(index, component);
    }

    public void ReplaceProjectile(long newCooldownTime) {
        var index = GameComponentsLookup.Projectile;
        var component = CreateComponent<ProjectileComponent>(index);
        component.cooldownTime = newCooldownTime;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectile() {
        RemoveComponent(GameComponentsLookup.Projectile);
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

    static Entitas.IMatcher<GameEntity> _matcherProjectile;

    public static Entitas.IMatcher<GameEntity> Projectile {
        get {
            if (_matcherProjectile == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Projectile);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProjectile = matcher;
            }

            return _matcherProjectile;
        }
    }
}
