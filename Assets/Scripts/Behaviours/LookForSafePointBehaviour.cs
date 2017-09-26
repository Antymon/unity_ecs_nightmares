using UnityEngine;
using Entitas;


public class LookForSafePointBehaviour : MonoBehaviour, IEntityDeserializer
{
    public int range = 100;

    public Transform threat;
    public Transform[] shelters;

    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;

    public void DeserializeEnitity(GameEntity entity)
    {
        //entity.ReplaceHealth(healthPoints, healthPointsCap);
    }

    public void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");

        shootRay.origin = threat.position;
        Vector3 direction;

        foreach (var shelterTransform in shelters)
        {
            direction = shelterTransform.position - threat.position;
            direction.y = 0f; //we are considering  floor XZ surface only
            shootRay.direction = direction;

            float checkedDistance = Mathf.Min(direction.magnitude,range);
            
            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, checkedDistance, shootableMask))
            {
                Debug.Log(shelterTransform.name + " safe");
            }
            else
            {
                Debug.Log(shelterTransform.name + " unsafe");
            }
        }
    }
}

