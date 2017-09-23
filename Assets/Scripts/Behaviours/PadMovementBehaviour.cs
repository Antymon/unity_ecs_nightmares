using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadMovementBehaviour : MonoBehaviour, IEntityDeserializer, BallChangedDirectionListener {

    private const int MIN_X = -10;
    private const int MAX_X = 10;

    private PadEntity padEntity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeserializeEnitity(Entitas.Entity entity)
    {
        padEntity = ((PadEntity)entity);
        padEntity.AddBallChangedDirectionListener(this);
    }

    public void DirectionChanged(Vector2 direction)
    {
        // Move the player to it's current position plus the movement.
        var position = transform.position;
        position.x += direction.x;

        position.x = Mathf.Max(position.x, MIN_X);
        position.x = Mathf.Min(position.x, MAX_X);

        transform.position = position;
    }
}
