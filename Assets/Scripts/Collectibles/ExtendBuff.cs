using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendBuff : Collectibles
{
  public float NewWidth = 2.5f;
    protected override void ApplyEffect()
    {
        if (PaddleMovement.Instance != null)
        {
            PaddleMovement.Instance.StartWidthAnimation(NewWidth);
        }
    }
}
