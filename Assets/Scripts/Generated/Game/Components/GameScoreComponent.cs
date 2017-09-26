//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity scoreEntity { get { return GetGroup(GameMatcher.Score).GetSingleEntity(); } }
    public ScoreComponent score { get { return scoreEntity.score; } }
    public bool hasScore { get { return scoreEntity != null; } }

    public GameEntity SetScore(int newCurrentScore) {
        if (hasScore) {
            throw new Entitas.EntitasException("Could not set Score!\n" + this + " already has an entity with ScoreComponent!",
                "You should check if the context already has a scoreEntity before setting it or use context.ReplaceScore().");
        }
        var entity = CreateEntity();
        entity.AddScore(newCurrentScore);
        return entity;
    }

    public void ReplaceScore(int newCurrentScore) {
        var entity = scoreEntity;
        if (entity == null) {
            entity = SetScore(newCurrentScore);
        } else {
            entity.ReplaceScore(newCurrentScore);
        }
    }

    public void RemoveScore() {
        scoreEntity.Destroy();
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

    public ScoreComponent score { get { return (ScoreComponent)GetComponent(GameComponentsLookup.Score); } }
    public bool hasScore { get { return HasComponent(GameComponentsLookup.Score); } }

    public void AddScore(int newCurrentScore) {
        var index = GameComponentsLookup.Score;
        var component = CreateComponent<ScoreComponent>(index);
        component.currentScore = newCurrentScore;
        AddComponent(index, component);
    }

    public void ReplaceScore(int newCurrentScore) {
        var index = GameComponentsLookup.Score;
        var component = CreateComponent<ScoreComponent>(index);
        component.currentScore = newCurrentScore;
        ReplaceComponent(index, component);
    }

    public void RemoveScore() {
        RemoveComponent(GameComponentsLookup.Score);
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

    static Entitas.IMatcher<GameEntity> _matcherScore;

    public static Entitas.IMatcher<GameEntity> Score {
        get {
            if (_matcherScore == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Score);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherScore = matcher;
            }

            return _matcherScore;
        }
    }
}