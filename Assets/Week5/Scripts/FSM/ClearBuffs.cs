using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearBuff")]
public class ClearBuffs : Action
{
    public override void Act(StateController controller)
    {
        BuffStateController m = (BuffStateController)controller;
        m.currentPowerupType = PowerupType.Default;
    }
}