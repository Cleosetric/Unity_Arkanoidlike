using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBuff : Collectibles
{
    public int addLife = 1;
    protected override void ApplyEffect()
    {
       GameManager.Instance.OnLifeBuff(addLife);
    }
}
