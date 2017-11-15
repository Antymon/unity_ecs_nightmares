using UnityEngine;
using Entitas;


public class GameFlowConfigBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int numberOfRounds;
    public int effectsAtTimeCap;
    public int roundTime;
    public int roundScoreReward;
    public int seed;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.ReplaceMatch(
            numberOfRounds,
            effectsAtTimeCap,
            roundTime,
            roundScoreReward,
            new SystemRandomAdapter(seed)); //unitys random is not portable (makes "ECall" into editor)

    }
}

