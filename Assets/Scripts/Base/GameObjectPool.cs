using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledGameObject
{
    void SetPool(IGameObjectPool pool);
    void Reset();
    GameObject gameObject { get; }
}

public interface IGameObjectPool
{
    GameObject Get(string typeId);
    void Return(GameObject poolItem);
}

//pool for game objects bound to entities
//binding itself is expected to be overwritten when reusing game object
public class BindableGameObjectPool : IGameObjectPool
{
    private Dictionary<string, Stack<GameObject>> pools;
    private IGameObjectFactory factory;

    public BindableGameObjectPool(IGameObjectFactory factory)
    {
        this.factory = factory;
        pools = new Dictionary<string, Stack<GameObject>>();
    }

    public GameObject Get(string id)
    {
        GameObject result;

        if (pools.ContainsKey(id) && pools[id].Count > 0)
        {
            result = pools[id].Pop();
        }
        else 
        {
            if (EntityPrefabNameBinding.idToBinding.ContainsKey(id) && EntityPrefabNameBinding.idToBinding[id].idIsPrefabName)
            {
                result = factory.Create(prefabName:id);
            }
            else
            {
                Debug.Log("Requested object is not in bindable pool and doesn't have prefab linked.");
                return null;
            }
        }

        var pooledObject = result.GetComponent<IPooledGameObject>();

        if (pooledObject == null)
        {
            Debug.LogError("Factory created object is not poolable.");
            UnityEngine.Object.Destroy(result);
            return null;
        }

        result.SetActive(true);

        pooledObject.SetPool(this);

        return result;
    }

    public void Return(GameObject poolGameObject)
    {
        var prefabIdentifier = poolGameObject.GetComponent<IPrefabIdentifier>();

        if (prefabIdentifier == null)
        {
            Debug.LogError("Attempt to return non-bindable GameObject failed.");
            return;
        }

        var pooledObjectsComponent = poolGameObject.GetComponent<IPooledGameObject>();

        if (pooledObjectsComponent == null)
        {
            Debug.LogError("Attempt to return not poolable GameObject failed.");
            return;
        }

        pooledObjectsComponent.Reset();

        var prefabBinding = prefabIdentifier.GetPrefabBinding();

        if (prefabBinding.canBeDisabled)
        {
            poolGameObject.SetActive(false);
        }

        var id = prefabBinding.id;

        if (!pools.ContainsKey(id))
        {
            pools[id] = new Stack<GameObject>();
        }

        var pool = pools[id];

        pool.Push(poolGameObject);
    }
}