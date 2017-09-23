//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity levelEntity { get { return GetGroup(GameMatcher.Level).GetSingleEntity(); } }
    public LevelComponent level { get { return levelEntity.level; } }
    public bool hasLevel { get { return levelEntity != null; } }

    public GameEntity SetLevel(int newNumberRounds, int newSpawnersCap, int newCurrentRound, int newWonByPlayer, long newRoundTime) {
        if (hasLevel) {
            throw new Entitas.EntitasException("Could not set Level!\n" + this + " already has an entity with LevelComponent!",
                "You should check if the context already has a levelEntity before setting it or use context.ReplaceLevel().");
        }
        var entity = CreateEntity();
        entity.AddLevel(newNumberRounds, newSpawnersCap, newCurrentRound, newWonByPlayer, newRoundTime);
        return entity;
    }

    public void ReplaceLevel(int newNumberRounds, int newSpawnersCap, int newCurrentRound, int newWonByPlayer, long newRoundTime) {
        var entity = levelEntity;
        if (entity == null) {
            entity = SetLevel(newNumberRounds, newSpawnersCap, newCurrentRound, newWonByPlayer, newRoundTime);
        } else {
            entity.ReplaceLevel(newNumberRounds, newSpawnersCap, newCurrentRound, newWonByPlayer, newRoundTime);
        }
    }

    public void RemoveLevel() {
        levelEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public LevelComponent level { get { return (LevelComponent)GetComponent(GameComponentsLookup.Level); } }
    public bool hasLevel { get { return HasComponent(GameComponentsLookup.Level); } }

    public void AddLevel(int newNumberRounds, int newSpawnersCap, int newCurrentRound, int newWonByPlayer, long newRoundTime) {
        var index = GameComponentsLookup.Level;
        var component = CreateComponent<LevelComponent>(index);
        component.numberRounds = newNumberRounds;
        component.spawnersCap = newSpawnersCap;
        component.currentRound = newCurrentRound;
        component.wonByPlayer = newWonByPlayer;
        component.roundTime = newRoundTime;
        AddComponent(index, component);
    }

    public void ReplaceLevel(int newNumberRounds, int newSpawnersCap, int newCurrentRound, int newWonByPlayer, long newRoundTime) {
        var index = GameComponentsLookup.Level;
        var component = CreateComponent<LevelComponent>(index);
        component.numberRounds = newNumberRounds;
        component.spawnersCap = newSpawnersCap;
        component.currentRound = newCurrentRound;
        component.wonByPlayer = newWonByPlayer;
        component.roundTime = newRoundTime;
        ReplaceComponent(index, component);
    }

    public void RemoveLevel() {
        RemoveComponent(GameComponentsLookup.Level);
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

    static Entitas.IMatcher<GameEntity> _matcherLevel;

    public static Entitas.IMatcher<GameEntity> Level {
        get {
            if (_matcherLevel == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Level);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLevel = matcher;
            }

            return _matcherLevel;
        }
    }
}
