//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity matchEntity { get { return GetGroup(GameMatcher.Match).GetSingleEntity(); } }
    public MatchComponent match { get { return matchEntity.match; } }
    public bool hasMatch { get { return matchEntity != null; } }

    public GameEntity SetMatch(int newNumberRounds, int newEffectsAtTimeCap, int newRoundTime, int newRoundScoreReward, int newSeed) {
        if (hasMatch) {
            throw new Entitas.EntitasException("Could not set Match!\n" + this + " already has an entity with MatchComponent!",
                "You should check if the context already has a matchEntity before setting it or use context.ReplaceMatch().");
        }
        var entity = CreateEntity();
        entity.AddMatch(newNumberRounds, newEffectsAtTimeCap, newRoundTime, newRoundScoreReward, newSeed);
        return entity;
    }

    public void ReplaceMatch(int newNumberRounds, int newEffectsAtTimeCap, int newRoundTime, int newRoundScoreReward, int newSeed) {
        var entity = matchEntity;
        if (entity == null) {
            entity = SetMatch(newNumberRounds, newEffectsAtTimeCap, newRoundTime, newRoundScoreReward, newSeed);
        } else {
            entity.ReplaceMatch(newNumberRounds, newEffectsAtTimeCap, newRoundTime, newRoundScoreReward, newSeed);
        }
    }

    public void RemoveMatch() {
        matchEntity.Destroy();
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

    public MatchComponent match { get { return (MatchComponent)GetComponent(GameComponentsLookup.Match); } }
    public bool hasMatch { get { return HasComponent(GameComponentsLookup.Match); } }

    public void AddMatch(int newNumberRounds, int newEffectsAtTimeCap, int newRoundTime, int newRoundScoreReward, int newSeed) {
        var index = GameComponentsLookup.Match;
        var component = CreateComponent<MatchComponent>(index);
        component.numberRounds = newNumberRounds;
        component.effectsAtTimeCap = newEffectsAtTimeCap;
        component.roundTime = newRoundTime;
        component.roundScoreReward = newRoundScoreReward;
        component.seed = newSeed;
        AddComponent(index, component);
    }

    public void ReplaceMatch(int newNumberRounds, int newEffectsAtTimeCap, int newRoundTime, int newRoundScoreReward, int newSeed) {
        var index = GameComponentsLookup.Match;
        var component = CreateComponent<MatchComponent>(index);
        component.numberRounds = newNumberRounds;
        component.effectsAtTimeCap = newEffectsAtTimeCap;
        component.roundTime = newRoundTime;
        component.roundScoreReward = newRoundScoreReward;
        component.seed = newSeed;
        ReplaceComponent(index, component);
    }

    public void RemoveMatch() {
        RemoveComponent(GameComponentsLookup.Match);
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

    static Entitas.IMatcher<GameEntity> _matcherMatch;

    public static Entitas.IMatcher<GameEntity> Match {
        get {
            if (_matcherMatch == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Match);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMatch = matcher;
            }

            return _matcherMatch;
        }
    }
}