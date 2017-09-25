using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public abstract class AbstractGameControllerBehaviour : MonoBehaviour {

    public Transform displayRoot;

    protected IEntityDeserializer entityDeserializer;

    private Systems systems;

	void Start () {
        Application.targetFrameRate = 60;

        IFactory factory = new Factory(displayRoot);
        IPool pool = new BindableGameObjectPool(factory);

        ReclaimInstatiatedPrefabs(displayRoot, pool);

        entityDeserializer = new EntityDeserializerViaBinding(pool);
        
        systems = new Feature("Systems");
        AddSystems(Contexts.sharedInstance, systems);
        systems.Initialize();
	}

    //template method just to separate config from general bootstrapping
    protected abstract void AddSystems(Contexts contexts, Systems systems);


    private void ReclaimInstatiatedPrefabs(Transform root, IPool pool)
    {
        var poolableObjects = root.GetComponentsInChildren<IPooledObject>();

        foreach (IPooledObject pooledObject in poolableObjects)
        {
            pool.Return(pooledObject.gameObject);
        }
    }

    void Update()
    {
        systems.Execute();
        systems.Cleanup();
    }

    void OnDestroy()
    {
        systems.TearDown();
    }
}
