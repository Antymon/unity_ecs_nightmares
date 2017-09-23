using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    void SetPool(IPool pool);
    void Reset();
    GameObject gameObject { get; }
}

public interface IPool
{
    GameObject Get(string typeId);
    void Return(GameObject poolItem);
}

public class BindableGameObjectPool : IPool
{
    private Dictionary<string, Stack<GameObject>> pools;
    private IFactory factory;

    public BindableGameObjectPool(IFactory factory)
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

        var pooledObject = result.GetComponent<IPooledObject>();

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

        var pooledObject = poolItem.GetComponent<IPooledObject>();

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