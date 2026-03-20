using UnityEngine;

public class PowerUpSlowMo : PowerUpBase
{
    protected override void ApplyEffect(PlayerController player)
        => player.ActivateSlowMo(duration);
}
