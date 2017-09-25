using Entitas;
using UnityEngine;

public class TransformResettingBindingBehaviour : BindingEntitasBehaviour
{
    private Vector3 position;
    private Quaternion rotation;

    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    public override void Reset()
    {
        base.Reset();
        transform.position = position;
        transform.rotation = rotation;

    }
}

