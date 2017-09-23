using Entitas;
using UnityEngine;

public abstract class AbstractEntitasBehaviour : MonoBehaviour, IEntitasBinding
{
    protected IPool pool;
    protected Entitas.Entity entity;

    public virtual void DeserializeEnitity(Entitas.Entity entity)
    {
        this.entity = entity;
        entity.OnDestroyEntity += OnDestroyHandler;
    }

    protected virtual void OnDestroyHandler(Entitas.IEntity entity)
    {
        entity.OnDestroyEntity -= OnDestroyHandler;

        pool.Return(this.gameObject);
    }

    public abstract EntityPrefabNameBinding GetPrefabBinding();

    public void SetPool(IPool pool)
    {
        this.pool = pool;
    }

    public virtual void Reset()
    {
        entity = null;
        pool = null;
    }

    public Entitas.Entity GetEntity()
    {
        return entity;
    }
}

