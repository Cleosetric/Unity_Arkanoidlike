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
	private Vector2 lastFrameVelocity;

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
        Invoke("StartBall",2);
	}
    
    void StartBall(){
		ShowTheBall();
        startGame = true;
        ball_rbody.velocity = Vector2.up * speed;
    }

	public void RestartBall(){
		SetParticles(false);
		sprite.enabled = true;
		startGame = false;

		InitialPosition();
		Invoke("StartBall",2);
	}

	void GameOverBall(){
		startGame = false;
        SetParticles(false);
		InitialPosition();
	}

    void Update()
    {
        if(!startGame){
            InitialPosition();
        }else{
			lastFrameVelocity = ball_rbody.velocity;
		}

		if(GameManager.Instance.IsGameOver() || GameManager.Instance.IsStageClear() ){
			GameOverBall();
		}
    }

	void HideTheBall() {
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

	void InitialPosition() {
		transform.position = new Vector2(box_racket.transform.position.x,box_racket.transform.position.y + 1);
	}

	void OnCollisionEnter2D(Collision2D obj) {
		directTheBall(obj);
		bounceTheBall(obj);
	}

	void bounceTheBall(Collision2D hit) {
		if(hit.gameObject.tag == "Wall" || hit.gameObject.tag == "Brick"  ){
			Vector2 inNormal = hit.contacts[0].normal;
			
			var lastSpeed = lastFrameVelocity.magnitude;
       		var direction = Vector2.Reflect(lastFrameVelocity.normalized, inNormal);
	        ball_rbody.velocity = direction * Mathf.Max(lastSpeed, speed);
		}
	}

	void directTheBall(Collision2D hit) {
	    if (hit.gameObject.tag == "Player" ) {
	        float x = hitFactor(transform.position,hit.transform.position,hit.collider.bounds.size.x);
	        Vector2 dir = new Vector2(x, 1).normalized;
	        ball_rbody.velocity = dir * speed;
	    }
	}
}
