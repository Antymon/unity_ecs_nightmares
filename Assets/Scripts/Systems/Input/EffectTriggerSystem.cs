using Entitas;
using System.Collections.Generic;

public class EffectTriggerSystem : ReactiveSystem<InputEntity>
{
    public EffectTriggerSystem(InputContext context)
        : base(context)
    {

    }

    protected override void Execute(List<InputEntity> entities)
    {
        GameEntity self;
        GameEntity other;

        foreach (var entity in entities)
        {
            self = entity.trigger.self;
            other = entity.trigger.other;

            ProcessTrigger(self, other, entity.trigger.onEnter);
            ProcessTrigger(other, self, entity.trigger.onEnter);

            entity.Destroy();
        }
    }

    private void ProcessTrigger(GameEntity agentEntity, GameEntity effectEntity, bool onEnter)
    {
        if(!agentEntity.hasAgent || !effectEntity.hasEffect)
        {
            return;
        }

        IEffect entityEffect = effectEntity.effect.effect;
        var aggentsEffects = agentEntity.agent.effects;

        if(onEnter) //trigger's volume was entered
        {
            if (entityEffect.IsApplicable(agentEntity))
            {
                //this effect is exclusive, cannot add it twice
                if (entityEffect.IsExclusive())
                {
                    foreach (var effect in aggentsEffects)
                    {
                        if (effect.GetType().Equals(entityEffect.GetType()))
                        {
                            return;
                        }
                    }
                }

                aggentsEffects.Add(entityEffect);
                if(entityEffect.IsCollectible())
                {
                    effectEntity.isMarkedToPostponedDestroy = true;
                }
            }
        }
        else //trigger volume was left
        {
            if (aggentsEffects.Contains(entityEffect))
            {
                aggentsEffects.Remove(entityEffect);
            }
        }

    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.Trigger.Added());
    }
}

