using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBuff : Collectibles
{
    protected override void ApplyEffect()
    {
        PaddleMovement.Instance.StartShooting();
    }
}
