
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "PluggableSM/Decisions/TransformBuff")]
public class TransformDecisionBuff : Decision
{
    public StateTransformMapBuff[] map;

    public override bool Decide(StateController controller)
    {
        BuffStateController m = (BuffStateController)controller;
        // we assume that the state is named (string matched) after one of possible values in MarioState
        // convert between current state name into MarioState enum value using custom class EnumExtension
        // you are free to modify this to your own use
        BuffState toCompareState = EnumExtension.ParseEnum<BuffState>(m.currentState.name);

        // loop through state transform and see if it matches the current transformation we are looking for
        for (int i = 0; i < map.Length; i++)
        {
            if (toCompareState == map[i].fromState && m.currentPowerupType == map[i].powerupCollected)
            {
                return true;
            }
        }

        return false;

    }
}

[System.Serializable]
public struct StateTransformMapBuff
{
    public BuffState fromState;
    public PowerupType powerupCollected;
}
