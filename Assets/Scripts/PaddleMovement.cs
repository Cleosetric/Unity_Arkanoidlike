using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public KeyCode moveLeft = KeyCode.A;
	public KeyCode moveRight = KeyCode.D;
	public float speed = 80f;
	private Rigidbody2D racket_rbody;
	private Vector3 initialPos;

	void Start () {
		racket_rbody = GetComponent<Rigidbody2D>();
		initialPos = transform.position;
	}
	 
	void FixedUpdate() {
		if(GameManager.Instance.IsGameOver() || GameManager.Instance.IsStageClear()){
			transform.position = initialPos;
		}else{
			makeMovableRacket();
		}
	}
	

	void makeMovableRacket() {
		var velocity = racket_rbody.velocity;
		if(Input.GetKey(moveLeft)){
			velocity.x = -speed;
		}else if(Input.GetKey(moveRight)){
			velocity.x = speed;
		}else{
			velocity.x = 0;
		}
		
		racket_rbody.velocity = velocity;
	}
}
