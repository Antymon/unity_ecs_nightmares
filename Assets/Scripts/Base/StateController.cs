using UnityEngine;
using System;
using System.Collections.Generic;

public class StateController<T>
{

    public enum Stage
    {
        NONE,
        ENTER,
        UPDATE,
        LEAVE
    }

    public T currentState { get; private set; }
    T nextState;
    bool nextStateSet;
    Stage stage;
    public float stateTime { get; private set; }
    public float elapsedTime { get; private set; }
    public float progress { get; private set; }

    Dictionary<T, Action<Stage, T>> handlers = new Dictionary<T, Action<Stage, T>>();
    Dictionary<T, float> times = new Dictionary<T, float>();
    Dictionary<T, T> transitions = new Dictionary<T, T>();
    Action<float> updateHandler;

    public StateController(T initial)
    {
        stage = Stage.ENTER;
        currentState = initial;
    }

    // public void AddState(T state, System.Func<Stage, T, bool> handler, bool initial = false)
    public void AddState(T state, Action<Stage, T> handler, float time = -1)
    {
        handlers.Add(state, handler);
        times.Add(state, time);
    }

    public void SetTransition(T stateFrom, T stateTo)
    {
        transitions.Add(stateFrom, stateTo);
    }

    public void ReEnterState()
    {
        stage = Stage.ENTER;
        nextState = currentState;
        nextStateSet = true;
    }

    // Go to next state now discarding current state completely
    public void EnterState(T nextState)
    {
        currentState = nextState;
        stage = Stage.ENTER;
    }

    // Set next state where we should transition from current state after
    // it ends
    public void SetNextState(T nextState, bool immediate = true)
    {
        this.nextState = nextState;
        nextStateSet = true;

        if (immediate || stage == Stage.NONE)
        {
            if (stage == Stage.UPDATE)
                stage = Stage.LEAVE;
            else
            {
                stage = Stage.ENTER;
                currentState = nextState;
            }
        }
    }

    public Stage Update(float deltaTime = 0f)
    {
        if (stage == Stage.NONE) return Stage.NONE;

        if (stage == Stage.LEAVE)
        {
            updateHandler = null;
            elapsedTime = 0;
            stateTime = 0;
            progress = 1;
            stage = Stage.ENTER;

            //Debug.Log (currentState + " " + Stage.LEAVE);
            handlers[currentState](Stage.LEAVE, currentState);

            if (nextStateSet)
            {
                currentState = nextState;
                nextStateSet = false;
            }
            else if (!transitions.ContainsKey(currentState))
            {
                stage = Stage.NONE;
            }
            else
            {
                currentState = transitions[currentState];
            }
            return stage;
        }

        if (stage == Stage.ENTER)
        {
            //if(times.ContainsKey(currentState)) {
            stateTime = times[currentState];
            //} else {
            //	stateTime = 0;
            //}
            progress = 0;
            elapsedTime = 0;
            stage = Stage.UPDATE;

            //Debug.Log (currentState + " " + Stage.ENTER);
            handlers[currentState](Stage.ENTER, currentState);
            return Stage.ENTER;
        }

        elapsedTime += deltaTime;
        if (stateTime > 0)
        {
            progress = elapsedTime / stateTime;
        }

        //Debug.Log (currentState + " " + stage);
        if (updateHandler != null)
        {
            updateHandler(progress);
        }
        else
        {
            handlers[currentState](stage, currentState);
        }

        if (stateTime >= 0 && elapsedTime >= stateTime)
        {
            //if(transitions.ContainsKey(currentState) || stateTime > 0)
            stage = Stage.LEAVE;
        }

        return stage;
    }

    public void SetUpdateHandler(Action<float> updateHandler)
    {
        this.updateHandler = updateHandler;
    }

    public void SetStateTime(float stateTime)
    {
        this.stateTime = stateTime;
    }

    public void SetStateTime(T state, float stateTime)
    {
        times[state] = stateTime;
    }

    public void EndState()
    {
        if (stage == Stage.NONE) return;
        //elapsedTime = stateTime;
        stage = Stage.LEAVE;
    }

    /*
    public void DelayState(float time) 
    {
        elapsedTime -= time;
    }
    */

    public bool NextStateSet()
    {
        return nextStateSet;
    }
}
