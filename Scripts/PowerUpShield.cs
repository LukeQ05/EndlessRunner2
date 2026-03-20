using UnityEngine;

public class PowerUpShield : PowerUpBase
{
    protected override void ApplyEffect(PlayerController player)
        => player.ActivateShield(duration);
}
