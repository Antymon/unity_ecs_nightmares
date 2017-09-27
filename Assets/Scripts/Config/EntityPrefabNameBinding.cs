﻿using System;
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
        ENEMY = 3,
        EFFECT_ADD_HEALTH = 4,
        EFFECT_PERSISTANT_ADD_HEALTH = 5,
        EFFECT_MOVEMENT_INVERTER = 6
    }

    public static Dictionary<Type, EntityPrefabNameBinding> entityTypeToPrefabName = new Dictionary<Type, EntityPrefabNameBinding>();

    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(Type.PLAYER, "Player");
    public static readonly EntityPrefabNameBinding JOYPAD_BINDING = new EntityPrefabNameBinding(Type.JOYPAD, "Joypad");
    public static readonly EntityPrefabNameBinding ENEMY_BINDING = new EntityPrefabNameBinding(Type.ENEMY, "Enemy");

    public static readonly EntityPrefabNameBinding EFFECT_ADD_HEALTH_BINDING = new EntityPrefabNameBinding(Type.EFFECT_ADD_HEALTH, "AddHealthEffect");
    public static readonly EntityPrefabNameBinding EFFECT_PERSISTANT_ADD_HEALTH_BINDING = new EntityPrefabNameBinding(Type.EFFECT_PERSISTANT_ADD_HEALTH, "PersistantAddHealthEffect");
    public static readonly EntityPrefabNameBinding EFFECT_MOVEMENT_INVERTER_BINDING = new EntityPrefabNameBinding(Type.EFFECT_MOVEMENT_INVERTER, "MovementInverterEffect");

    public Type entityType;
    public string prefabName;


    private EntityPrefabNameBinding(Type entityType, string prefabName)
    {

        entityTypeToPrefabName.Add(entityType, this);

        this.entityType = entityType;
        this.prefabName = prefabName;
    }


}