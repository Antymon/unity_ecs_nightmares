using Entitas;
using UnityEngine;

public class TransformResettingBindingBehaviour : BindingEntitasBehaviour
{
    private Vector3 position;
    private Quaternion rotation;
    private bool transformCached = false;

    public void Awake()
    {
        CacheTransform();
    }

    public override void DeserializeEnitity(GameEntity entity)
    {
        CacheTransform();
 	    base.DeserializeEnitity(entity);
    }

    //depending on instantiation - unity or factory method, one or other start point may be called
    private void CacheTransform()
    {
        if(transformCached)
        {
            return;
        }
        position = transform.position;
        rotation = transform.rotation;
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = position;
        transform.rotation = rotation;

    }
}

