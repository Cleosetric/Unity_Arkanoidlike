using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {
	public float speed = 30;
	public Transform box_racket;
	private Rigidbody2D ball_rbody;
	private ParticleSystem particle;
	private SpriteRenderer sprite;
	private bool startGame = false;

	float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth) {
	    return (ballPos.x - racketPos.x) / racketWidth;
	}

    void Awake()
    {
        ball_rbody = GetComponent<Rigidbody2D>();
        particle = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        
    }
    
	void Start () {
		ShowTheBall();
        SetParticles(false);
        Invoke("StartGame",2);
	}
    
    void StartGame(){
        SetParticles(true);
        startGame = true;
        ball_rbody.velocity = Vector2.up * speed;
    }

    void Update()
    {
        if(!startGame){
            ResetPosition();
        }
    }

	void hideTheBall() {
        SetParticles(false);
		sprite.enabled = false;
	}
    
	void ShowTheBall() {
        SetParticles(true);
		sprite.enabled = true;
	}

    void SetParticles(bool bolean){
        var em = particle.emission;
		em.enabled = bolean;
    }

	void ResetPosition() {
		transform.position = new Vector2(box_racket.transform.position.x,box_racket.transform.position.y + 0.5f);
	}

	void OnCollisionEnter2D(Collision2D paddle) {
		directTheBall(paddle);
		// bounceTheBall(paddle);
	}

	void bounceTheBall(Collision2D hit) {
		if(hit.gameObject.name == "Paddle"){
			Vector2 vel;
			vel.x = ball_rbody.velocity.x + hit.collider.attachedRigidbody.velocity.x;
			vel.y = ball_rbody.velocity.y;
			ball_rbody.velocity = vel;
		}
	}

	void directTheBall(Collision2D hit) {
	    if (hit.gameObject.name == "Paddle") {
	        float x = hitFactor(transform.position,hit.transform.position,hit.collider.bounds.size.x);
	        Vector2 dir = new Vector2(x, 1).normalized;
	        ball_rbody.velocity = dir * speed;
	    }
	}
}
