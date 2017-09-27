using Entitas;

public class PersistantAddHealthEffectBehaviour : TransformResettingBindingBehaviour
{
    public int healthPoints;
    public ulong intervalTicks; //how often to apply effect

    public override void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        transform.position = entity.position.position;

        PersistantAddHealthEffect entityEffect = ((PersistantAddHealthEffect)entity.effect.effect);

        entityEffect.healthPoints = healthPoints;
        entityEffect.intervalTicks = intervalTicks;
        
    }
}

