using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObjectFactory
{
    GameObject Create(string prefabName);
}

public class GameObjectFactory : IGameObjectFactory
{
    private Dictionary<string, GameObject> resources;
    private Transform defaultTransform;

    //transform will be used as a parent for spawning game objects
    public GameObjectFactory(Transform defaultTransform)
    {
        resources = new Dictionary<string, GameObject>();
        this.defaultTransform = defaultTransform;
    }

    public GameObject Create(string prefabName)
    {
        if (!resources.ContainsKey(prefabName))
        {
            resources.Add(prefabName, Resources.Load<GameObject>(prefabName));
        }

        var relatedGO = UnityEngine.Object.Instantiate<GameObject>(resources[prefabName],defaultTransform);

        return relatedGO;
    }
}
