using Entitas;
using UnityEngine;

public class BindingEntitasBehaviour : MonoBehaviour, IEntitasBinding
{
    public EntityPrefabNameBinding.Type entityPrefabNameBinding;

    protected IGameObjectPool pool;
    protected GameEntity entity;

    public virtual void DeserializeEnitity(GameEntity entity)
    {
        this.entity = entity;
        entity.OnDestroyEntity += OnDestroyHandler;
    }

    protected virtual void OnDestroyHandler(Entitas.IEntity entity)
    {
        entity.OnDestroyEntity -= OnDestroyHandler;

        pool.Return(this.gameObject);
    }

    public EntityPrefabNameBinding GetPrefabBinding()
    {
        return EntityPrefabNameBinding.entityTypeToPrefabName[entityPrefabNameBinding];
    }

    public void SetPool(IGameObjectPool pool)
    {
        this.pool = pool;
    }

    public virtual void Reset()
    {
        entity = null;
        pool = null;
    }

    public GameEntity GetEntity()
    {
        return entity;
    }
}

