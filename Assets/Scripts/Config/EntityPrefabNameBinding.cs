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
        ENEMY = 3,

        EFFECT_ADD_HEALTH = 4,
        EFFECT_PERSISTANT_ADD_HEALTH = 5,
        EFFECT_MOVEMENT_INVERTER = 6,

        GAME_OVER_SCREEN = 7,
        PAUSE_SCREEN = 8,
        HEALTH_BAR = 9,
        SCORE_COUNTER = 10,
        ROUND_COUNTER = 11,

        GAME_FLOW_CONFIG = 12,
    }

    public static Dictionary<Type, EntityPrefabNameBinding> entityTypeToBinding = new Dictionary<Type, EntityPrefabNameBinding>();
    public static Dictionary<string, EntityPrefabNameBinding> idToBinding = new Dictionary<string, EntityPrefabNameBinding>();


    public static readonly EntityPrefabNameBinding PLAYER_BINDING = new EntityPrefabNameBinding(Type.PLAYER, "Player");
    public static readonly EntityPrefabNameBinding JOYPAD_BINDING = new EntityPrefabNameBinding(Type.JOYPAD, "Joypad");
    public static readonly EntityPrefabNameBinding ENEMY_BINDING = new EntityPrefabNameBinding(Type.ENEMY, "Enemy");

    public static readonly EntityPrefabNameBinding EFFECT_ADD_HEALTH_BINDING = new EntityPrefabNameBinding(Type.EFFECT_ADD_HEALTH, "AddHealthEffect");
    public static readonly EntityPrefabNameBinding EFFECT_PERSISTANT_ADD_HEALTH_BINDING = new EntityPrefabNameBinding(Type.EFFECT_PERSISTANT_ADD_HEALTH, "PersistantAddHealthEffect");
    public static readonly EntityPrefabNameBinding EFFECT_MOVEMENT_INVERTER_BINDING = new EntityPrefabNameBinding(Type.EFFECT_MOVEMENT_INVERTER, "MovementInverterEffect");

    public static readonly EntityPrefabNameBinding GAME_OVER_SCREEN_BINDING = new EntityPrefabNameBinding(Type.GAME_OVER_SCREEN, "GameOverScreen");
    public static readonly EntityPrefabNameBinding PAUSE_SCREEN_BINDING = new EntityPrefabNameBinding(Type.PAUSE_SCREEN, "PauseScreen");
    public static readonly EntityPrefabNameBinding HEALTH_BAR_BINDING = new EntityPrefabNameBinding(Type.HEALTH_BAR, "HealthBar");
    public static readonly EntityPrefabNameBinding SCORE_COUNTER_BINDING = new EntityPrefabNameBinding(Type.SCORE_COUNTER, "ScoreCounter");
    public static readonly EntityPrefabNameBinding ROUND_COUNTER_BINDING = new EntityPrefabNameBinding(Type.ROUND_COUNTER, "RoundCounter");

    public static readonly EntityPrefabNameBinding GAME_FLOW_CONFIG_BINDING = new EntityPrefabNameBinding(Type.GAME_FLOW_CONFIG, "GameFlowConfig", false, false);


    public Type entityType;
    public bool idIsPrefabName;
    public string id;
    public bool canBeDisabled;

    private EntityPrefabNameBinding(Type entityType, string id, bool idIsPrefabName = true, bool canBeDisabled = true)
    {
        idToBinding.Add(id, this);
        entityTypeToBinding.Add(entityType, this);

        this.entityType = entityType;
        this.idIsPrefabName = idIsPrefabName;
        this.id = id;
        this.canBeDisabled = canBeDisabled;
    }


}
