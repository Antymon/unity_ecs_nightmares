using Entitas;

public class MovementInverterEffectBehaviour : TransformResettingBindingBehaviour
{
    public int lastingTicks; //how long will the inverter last

    public override void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        MovementInverterEffect entityEffect = ((MovementInverterEffect)entity.effect.effect);

        entityEffect.lastingTicks = lastingTicks;       
    }
}

