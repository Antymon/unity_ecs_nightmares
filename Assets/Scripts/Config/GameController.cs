using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

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

    //in general systems ordering is significant
    private void AddSystems(Contexts contexts, Systems systems)
    {
        systems.Add(new PlayerInitSystem(contexts.game, entityDeserializer));
        systems.Add(new JoypadSystem(contexts.input,contexts.game, entityDeserializer));
        systems.Add(new PlayerMovementSystem(contexts.game));
        systems.Add(new TriggerBulletSystem(contexts.input, contexts.game));
        systems.Add(new ShootingSystem(contexts.game));
    }

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
