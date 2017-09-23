using System;
using System.Collections.Generic;

public class EntityPrefabNameBinding
{
    public static Dictionary<System.Type, EntityPrefabNameBinding> entityTypeToPrefabName = new Dictionary<System.Type, EntityPrefabNameBinding>();

    /*
    Regex bindings generation:
	Input:NAME Name
	Match:(.*) (.*)
	Replace:    public static readonly EntityPrefabNameBinding $1_BINDING = new EntityPrefabNameBinding\(typeof\($2Entity\), "$2"\);
	Output:    public static readonly EntityPrefabNameBinding NAME_BINDING = new EntityPrefabNameBinding(typeof(NameEntity), "Name");
    */

    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(typeof(PlayerEntity), "Player");

    public System.Type entityType;
    public string prefabName;
    

    private EntityPrefabNameBinding(System.Type entityType, string prefabName)
    {

        entityTypeToPrefabName.Add(entityType, this);

        this.entityType = entityType;
        this.prefabName = prefabName;
    }
}
