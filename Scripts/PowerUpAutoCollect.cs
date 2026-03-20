using UnityEngine;

public class PowerUpAutoCollect : PowerUpBase
{
    protected override void ApplyEffect(PlayerController player)
        => player.ActivateAutoCollect(duration);
}
