﻿using DG.Tweening;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    private Tween roundTimer;

    private GameContext gameContext;
    private InputContext inputContext;

    private LevelComponent levelComponent;

    private GameEntity player;
    private GameEntity enemy;

    public RoundSystem(GameContext context, InputContext inputContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = context;
        this.inputContext = inputContext;

        //ToDo: deserialize from outside like rest
        context.SetLevel(
            newNumberRounds: 5, 
            newEffectsAtTimeCap: 4, 
            newCurrentRound: 0, 
            newRoundTime: 180, 
            newRoundScoreReward: 1,
            newSeed: 0);

        //ToDo: unitys random is not portable, replace
        Random.InitState(context.level.seed);

        levelComponent = context.level;
    }

    public void Initialize()
    {
        gameContext.GetGroup(GameMatcher.GameStart).OnEntityAdded += OnGameStart;
        var gameStartEntity = gameContext.CreateEntity();
        gameStartEntity.isGameStart = true;
        gameStartEntity.isMarkedToPostponedDestroy = true;
        
    }

    private void OnGameStart(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        entity.isGameStart = false;


        gameContext.ReplaceScores(new Dictionary<int, int>());


        levelComponent.currentRound = 0;
        StartNextRound();
    }

    private void StartNextRound()
    {
        gameContext.CreateEntity().isRoundStarted = true;

        levelComponent.currentRound++;

        roundTimer = DOVirtual.DelayedCall(levelComponent.roundTime, FinishRound);
        roundTimer.Play();

        CreateAgents();
    }

    private void CreateAgents()
    {
        player = RequestActorCreation(player: true, target: null);
        enemy = RequestActorCreation(player: false, target: player);

        player.agent.target = enemy;

        

        //setting following flags will make them picked up synchronously
        //so they are set when everything is ready
        player.isPlayer = true;
        enemy.isEnemy = true;
    }

    private GameEntity RequestActorCreation(bool player, GameEntity target)
    {
        var binding = player ? EntityPrefabNameBinding.PLAYER_BINDING : EntityPrefabNameBinding.ENEMY_BINDING;
        var agentEntity = gameContext.CreateEntity();
        agentEntity.AddEntityBinding(binding);

        //agent placeholder values
        agentEntity.AddAgent(
            newId: 0, 
            newName: string.Empty, 
            newScore: 0, 
            newEffects: new List<IEffect>(), 
            newTarget: target
            );

        entityDeserializer.DeserializeEnitity(agentEntity);

        InitializeScoreForAgent(agentEntity);

        agentEntity.AddPositionChanged(inputContext.tick.currentTick, 0, false, agentEntity.position.position);

        return agentEntity;
    }

    private void InitializeScoreForAgent(GameEntity agentEntity)
    {
        var scores = gameContext.scores.agentIdToScoreMapping;
        var agentId = agentEntity.agent.id;

        if (!scores.ContainsKey(agentId))
        {
            scores[agentId] = 0;
        }
    }

    protected override void Execute(List<GameEntity> entities)
    {
        bool playerIsDead = false;
        bool enemyIsDead = false;

        GameEntity agent;

        foreach (var entity in entities)
        {
            agent = entity.agentDead.agent;

            if (agent.isPlayer)
            {
                playerIsDead = true;
            }
            else if (agent.isEnemy)
            {
                enemyIsDead = true;
            }
            entity.Destroy();
        }

        if (playerIsDead)
        {
            OnAgentWon(enemy);
        }
        else if (enemyIsDead)
        {
            OnAgentWon(player);
        }
    }

    private void OnAgentWon(GameEntity agentEntity)
    {
        gameContext.scores.agentIdToScoreMapping[agentEntity.agent.id]+=levelComponent.roundScoreReward;
        FinishRound();
    }

    private void FinishRound()
    {
        gameContext.CreateEntity().isRoundFinished = true;

        roundTimer.Kill();

        if(player.isEnabled)
            player.Destroy();

        if(enemy.isEnabled)
            enemy.Destroy();

        if (levelComponent.currentRound < levelComponent.numberRounds)
        {
            StartNextRound();
        }
        else
        {
            gameContext.CreateEntity().isGameOver = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.AgentDead.Added());
    }
}