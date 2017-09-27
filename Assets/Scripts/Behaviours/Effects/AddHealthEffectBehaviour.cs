using Entitas;

public class AddHealthEffectBehaviour : TransformResettingBindingBehaviour
{
    public int healthPoints;

    public override void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        transform.position = entity.position.position;

        ((AddHealthEffect)entity.effect.effect).healthPoints = healthPoints;
    }
}

