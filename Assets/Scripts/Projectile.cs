using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Brick")
        {
            BlockEvent brick = hit.gameObject.GetComponent<BlockEvent>();
            brick.CheckBrickGetHit();
        }
        Destroy(gameObject);
    }

}
