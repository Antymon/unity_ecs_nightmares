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

    public GameObject Get(string prefabName)
    {
        GameObject result;

        if (pools.ContainsKey(prefabName) && pools[prefabName].Count > 0)
        {
            result = pools[prefabName].Pop();
        }
        else
        {
            result = factory.Create(prefabName);
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

    public void Return(GameObject poolItem)
    {
        var prefabIdentifier = poolItem.GetComponent<IPrefabIdentifier>();

        if (prefabIdentifier == null)
        {
            Debug.LogError("Attempt to return non-bindable GameObject failed.");
            return;
        }

        var pooledObject = poolItem.GetComponent<IPooledGameObject>();

        if (pooledObject == null)
        {
            Debug.LogError("Attempt to return not poolable GameObject failed.");
            return;
        }

        pooledObject.Reset();

        poolItem.SetActive(false);

        var prefabName = prefabIdentifier.GetPrefabBinding().prefabName;

        if (!pools.ContainsKey(prefabName))
        {
            pools[prefabName] = new Stack<GameObject>();
        }

        var pool = pools[prefabName];

        pool.Push(poolItem);
    }
}