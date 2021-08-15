using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
     private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Ball")
        {
            BallMovement ball = hit.GetComponent<BallMovement>();
            GameManager.Instance.OnBallDeath();
            ball.RestartBall();
            
        }
    }
}
