using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    BallMovement ball;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>();
    }

     private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == "Ball" ||  hit.gameObject.tag == "Mob")
        {
            AudioManager.instance.Play("LoseLive");
            GameManager.Instance.OnBallDeath();
            ball.RestartBall();
        }
    }
}
