
using Entitas;
using System.Collections.Generic;

public class PersistantAddHealthEffect : IEffect
{
    public int healthPoints;
    public int intervalTicks; //how many updates needed to reapply

    private Dictionary<int, int> userIdsToTicksSinceApply; //agents id who are/were using persistant effect
    private List<int> userIds; //optimization, keys from userIdsToTicksSinceApply

    public PersistantAddHealthEffect()
    {
        userIdsToTicksSinceApply = new Dictionary<int, int>();
        userIds = new List<int>();
    }

    public void Apply(GameEntity entity)
    {
        if(!CanApply(entity))
        {
            return;
        }

        //ToDo: health points capping logic shouldn't be here
        entity.health.healthPoints = System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);

        userIdsToTicksSinceApply[entity.agent.id] = 0;
    }

    public bool IsUsed()
    {
        return false;
    }

    public bool CanApply(GameEntity entity)
    {
        return entity.hasAgent && GetTicksForAgent(entity.agent)>= intervalTicks;
    }

    public void Update()
    {
        foreach(var id in userIds)
        {
            userIdsToTicksSinceApply[id]++;
        }
    }

    public bool IsCollectible()
    {
        return false;
    }

    private int GetTicksForAgent(AgentComponent agentComponent)
    {
        //intervalTicks is returned to satisfy first use condition of CanApply
        if(!userIdsToTicksSinceApply.ContainsKey(agentComponent.id))
        {
            userIdsToTicksSinceApply[agentComponent.id] = intervalTicks;
            userIds.Add(agentComponent.id);
        }

        return userIdsToTicksSinceApply[agentComponent.id];
    }
}

