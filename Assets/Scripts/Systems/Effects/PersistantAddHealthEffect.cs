
using Entitas;
using System.Collections.Generic;

public class PersistantAddHealthEffect : IEffect
{
    public int healthPoints;
    public ulong intervalTicks; //how many updates needed to reapply

    private Dictionary<int, ulong> userIdsToApplicationTick; //agents id who are/were using persistant effect

    private ulong currentTick = 0;

    public PersistantAddHealthEffect()
    {
        userIdsToApplicationTick = new Dictionary<int, ulong>();
    }

    public bool Apply(GameEntity entity)
    {
        if(!CanApply(entity))
        {
            return false;
        }

        //ToDo: health points capping logic shouldn't be here
        entity.health.healthPoints = System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);

        userIdsToApplicationTick[entity.agent.id] = currentTick;

        return true;
    }

    public bool IsUsed()
    {
        return false;
    }

    private bool CanApply(GameEntity entity)
    {
        return IsApplicable(entity) && GetTicksForAgent(entity.agent) >= intervalTicks;
    }

    public bool IsApplicable(GameEntity entity)
    {
        return entity.hasAgent && entity.hasHealth;
    }

    public void Update(ulong time)
    {
        currentTick = time;
    }

    public bool IsCollectible()
    {
        return false;
    }

    private ulong GetTicksForAgent(AgentComponent agentComponent)
    {
        //intervalTicks is returned to satisfy first use condition of CanApply
        if(!userIdsToApplicationTick.ContainsKey(agentComponent.id))
        {
            return intervalTicks;
        }

        return currentTick - userIdsToApplicationTick[agentComponent.id];
    }

    public bool IsExclusive()
    {
        return false;
    }
}

