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
            seed);
    }
}

