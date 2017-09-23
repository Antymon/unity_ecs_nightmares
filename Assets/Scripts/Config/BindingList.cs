using System;
using System.Collections.Generic;

public class EntityPrefabNameBinding
{
    public static Dictionary<System.Type, EntityPrefabNameBinding> entityTypeToPrefabName = new Dictionary<System.Type, EntityPrefabNameBinding>();

    //public static readonly EntityPrefabNameBinding BACKGROUND_BINDING = new EntityPrefabNameBinding(typeof(BackgroundEntity), "Background");

    public System.Type entityType;
    public string prefabName;
    

    private EntityPrefabNameBinding(System.Type entityType, string prefabName)
    {

        entityTypeToPrefabName.Add(entityType, this);

        this.entityType = entityType;
        this.prefabName = prefabName;
    }
}
