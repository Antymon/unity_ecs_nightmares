//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity scoresEntity { get { return GetGroup(GameMatcher.Scores).GetSingleEntity(); } }
    public ScoresComponent scores { get { return scoresEntity.scores; } }
    public bool hasScores { get { return scoresEntity != null; } }

    public GameEntity SetScores(System.Collections.Generic.Dictionary<int, int> newAgentIdToScoreMapping) {
        if (hasScores) {
            throw new Entitas.EntitasException("Could not set Scores!\n" + this + " already has an entity with ScoresComponent!",
                "You should check if the context already has a scoresEntity before setting it or use context.ReplaceScores().");
        }
        var entity = CreateEntity();
        entity.AddScores(newAgentIdToScoreMapping);
        return entity;
    }

    public void ReplaceScores(System.Collections.Generic.Dictionary<int, int> newAgentIdToScoreMapping) {
        var entity = scoresEntity;
        if (entity == null) {
            entity = SetScores(newAgentIdToScoreMapping);
        } else {
            entity.ReplaceScores(newAgentIdToScoreMapping);
        }
    }

    public void RemoveScores() {
        scoresEntity.Destroy();
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

    public ScoresComponent scores { get { return (ScoresComponent)GetComponent(GameComponentsLookup.Scores); } }
    public bool hasScores { get { return HasComponent(GameComponentsLookup.Scores); } }

    public void AddScores(System.Collections.Generic.Dictionary<int, int> newAgentIdToScoreMapping) {
        var index = GameComponentsLookup.Scores;
        var component = CreateComponent<ScoresComponent>(index);
        component.agentIdToScoreMapping = newAgentIdToScoreMapping;
        AddComponent(index, component);
    }

    public void ReplaceScores(System.Collections.Generic.Dictionary<int, int> newAgentIdToScoreMapping) {
        var index = GameComponentsLookup.Scores;
        var component = CreateComponent<ScoresComponent>(index);
        component.agentIdToScoreMapping = newAgentIdToScoreMapping;
        ReplaceComponent(index, component);
    }

    public void RemoveScores() {
        RemoveComponent(GameComponentsLookup.Scores);
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

    static Entitas.IMatcher<GameEntity> _matcherScores;

    public static Entitas.IMatcher<GameEntity> Scores {
        get {
            if (_matcherScores == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Scores);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherScores = matcher;
            }

            return _matcherScores;
        }
    }
}
