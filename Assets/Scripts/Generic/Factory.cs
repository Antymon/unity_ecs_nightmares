using System;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory
{
    GameObject Create(string prefabName);
}

public class Factory : IFactory
{
    private Dictionary<string, GameObject> resources;
    private Transform defaultTransform;

    public Factory(Transform defaultTransform)
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
