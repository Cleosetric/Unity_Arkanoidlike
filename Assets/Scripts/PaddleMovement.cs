using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
	#region Singleton

    private static PaddleMovement _instance;

    public static PaddleMovement Instance => _instance;

    public bool PaddleIsTransforming { get; set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion


    public KeyCode moveLeft = KeyCode.A;
	public KeyCode moveRight = KeyCode.D;
	public float speed = 80f;
	private float paddleWidth = 1.75f;
	private Rigidbody2D racket_rbody;
	private SpriteRenderer sprite;
	private Vector3 initialPos;

	// Shooting
    public bool PaddleIsShooting { get; set; }
    public GameObject leftMuzzle;
    public GameObject rightMuzzle;
    public Projectile bulletPrefab;

	void Start () {
		racket_rbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		sprite.drawMode = SpriteDrawMode.Tiled;
		initialPos = transform.position;
		this.PaddleIsTransforming = false;
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

	public void StartWidthAnimation(float newWidth)
    {
		if(!this.PaddleIsTransforming){
			StartCoroutine(AnimatePaddleWidth(newWidth));
		}
    }

    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        this.StartCoroutine(ResetPaddleWidthAfterTime(5));

        if (width > this.sprite.size.x)
        {
            float currentWidth = this.sprite.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sprite.size = new Vector2(currentWidth, 0.64f);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sprite.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sprite.size = new Vector2(currentWidth, 0.64f);
                yield return null;
            }
        }
		this.PaddleIsTransforming = false;
    }

	private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }

	public void StartShooting()
    {
        if (!this.PaddleIsShooting)
        {
            this.PaddleIsShooting = true;
            StartCoroutine(StartShootingRoutine());
        }
    }

	public IEnumerator StartShootingRoutine()
    {
        float fireCooldown = 1.5f; 
        float fireCooldownLeft = 0;

        float shootingDuration = 10;
        float shootingDurationLeft = shootingDuration;

        while (shootingDurationLeft >= 0)
        {
            fireCooldownLeft -= Time.deltaTime;
            shootingDurationLeft -= Time.deltaTime;

            if (fireCooldownLeft <= 0)
            {
                this.Shoot();
                fireCooldownLeft = fireCooldown;
            }

            yield return null;
        }

        this.PaddleIsShooting = false;
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);
    }

	private void Shoot()
    {
        AudioManager.instance.Play("Arrow");
        leftMuzzle.SetActive(false);
        rightMuzzle.SetActive(false);

        leftMuzzle.SetActive(true);
        rightMuzzle.SetActive(true);

        this.SpawnBullet(leftMuzzle);
        this.SpawnBullet(rightMuzzle);
    }

	private void SpawnBullet(GameObject muzzle)
    {
        Vector3 spawnPosition = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y + 2f, muzzle.transform.position.z);
        Projectile bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(new Vector2(0, 450f));
    }
}
