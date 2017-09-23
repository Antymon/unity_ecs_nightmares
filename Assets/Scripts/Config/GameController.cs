using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

    public Transform displayRoot;

    Systems systems;

    IEntityDeserializer entityDeserializer;

	void Start () {
        Application.targetFrameRate = 60;

        IPool pool = new BindableGameObjectPool(new Factory(displayRoot));

        ReclaimInstatiatedPrefabs(displayRoot, pool);

        entityDeserializer = new EntityDeserializerViaBinding(pool);
        
        var contexts = Contexts.sharedInstance;
        systems = new Feature("Systems");
        systems.Add(new BallSpawnerSystem(contexts.ball, entityDeserializer));
        systems.Add(new BallMovementSystem());
        systems.Add(new PadCollisionSystem(entityDeserializer));
        systems.Add(new KeyInputSystem());
        systems.Add(new BackgroundSystem(entityDeserializer));
        systems.Add(new BlockCollisionSystem(entityDeserializer));
        systems.Add(new InputCollisionSystem());
        systems.Initialize();
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
