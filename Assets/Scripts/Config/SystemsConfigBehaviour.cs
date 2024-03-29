﻿using Entitas;
using UnityEngine;

public class SystemsConfigBehaviour : AbstractGameControllerBehaviour
{
    //in general systems ordering is significant

    protected override void AddSystems(Contexts contexts, Systems systems)
    {
        var gameContext = contexts.game;
        var inputContext = contexts.input;

        //intialization dependency-free, execution may be dependant
        systems.Add(new TickSystem(inputContext));
        systems.Add(new ShootingSystem(gameContext));
        systems.Add(new DestroySystem(gameContext));
        systems.Add(new EnemyAISystem(gameContext));

        systems.Add(new UISystem(gameContext, entityDeserializer));

        systems.Add(new EffectTriggerSystem(inputContext));
        systems.Add(new CollisionSystem(inputContext, gameContext));
        systems.Add(new PlayerControlsSystem(inputContext, gameContext, entityDeserializer));

        //tick dependant
        systems.Add(new MovementSystem(gameContext, inputContext));
        systems.Add(new EffectSystem(gameContext, inputContext, entityDeserializer));

        //entry point system, generally would depend on everything except for unrelated inputs
        systems.Add(new GameFlowSystem(gameContext, inputContext, entityDeserializer));
    }
}

