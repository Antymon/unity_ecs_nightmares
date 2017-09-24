using System;
using System.Collections.Generic;

/*
 * Purpose of this class is to eliminate magic values for prefab names
 * and provide mapping to enum type which is more easy to use for instance
 * when assigning values in inspector
 * 
 * Bindings generation:
	Input:PLAYER Player
	Match:(.*) (.*)
	Replace:    public static readonly EntityPrefabNameBinding $1_BINDING = new EntityPrefabNameBinding\(Type.$1, "$2"\);
	Output:    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(Type.PLAYER, "Player");
 * 
 */

public class EntityPrefabNameBinding
{
    public enum Type
    {
        PLAYER = 1,
        JOYPAD = 2,
    }

    public static Dictionary<Type, EntityPrefabNameBinding> entityTypeToPrefabName = new Dictionary<Type, EntityPrefabNameBinding>();

    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(Type.PLAYER, "Player");
    public static readonly EntityPrefabNameBinding JOYPAD_BINDING = new EntityPrefabNameBinding(Type.JOYPAD, "Joypad");

    public Type entityType;
    public string prefabName;


    private EntityPrefabNameBinding(Type entityType, string prefabName)
    {

        entityTypeToPrefabName.Add(entityType, this);

        this.entityType = entityType;
        this.prefabName = prefabName;
    }


}
