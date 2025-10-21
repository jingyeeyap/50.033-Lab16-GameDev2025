using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffState
{
    Default = -1,
    invincible = 0
}

public class BuffStateController : StateController
{
    public PowerupType currentPowerupType = PowerupType.Default;
    public BuffState shouldBeNextState = BuffState.Default;

    public override void Start()
    {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart()
    {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType i)
    {
        currentPowerupType = i;
    }

    public void Star()
    {
        this.currentState.DoEventTriggeredActions(this, ActionType.Default);
    }

}