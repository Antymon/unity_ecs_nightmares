using UnityEngine;
using Entitas;


public class AgentBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int agentId;
    public string agentName;

    public void DeserializeEnitity(GameEntity entity)
    {
        entity.agent.id = agentId;
        entity.agent.name = agentName;
    }
}

