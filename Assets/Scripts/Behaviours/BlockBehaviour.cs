using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : AbstractEntitasBehaviour
{
    public override EntityPrefabNameBinding GetPrefabBinding()
    {
        return EntityPrefabNameBinding.BLOCK_BINDING;
    }
}
