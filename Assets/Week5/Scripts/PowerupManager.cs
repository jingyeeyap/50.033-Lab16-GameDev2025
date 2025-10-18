using UnityEngine;
using UnityEngine.Events;

public class PowerUpManager : MonoBehaviour
{
    public UnityEvent<IPowerup> powerupAffectsManager;
    public UnityEvent<IPowerup> powerupAffectsPlayer;

    public void FilterAndCastPowerup(IPowerup powerup)
    {
        switch (powerup.powerupType)
        {
            case PowerupType.Coin:
                powerupAffectsManager.Invoke(powerup);
                break;
            case PowerupType.MagicMushroom:
                powerupAffectsPlayer.Invoke(powerup);
                break;
        }
    }
}