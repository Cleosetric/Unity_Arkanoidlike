using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEvent : MonoBehaviour
{
    public int blockLives = 1;
    private int currentLive;
    public Sprite spriteDamaged;

    [Header("Shake Settings")]
    [Range(0f, 2f)]
    public float time = 0.2f;
    [Range(0f, 2f)]
    public float distance = 0.1f;
    [Range(0f, 0.1f)]
    public float delayShake = 0f;


    private Vector3 blockPos;
    private float timer;
    private Vector3 randomPos;
    private SpriteRenderer spriteBlock;
    private ParticleSystem particle;
    
    void Awake()
    {
        spriteBlock = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }
    void Start()
    {
        blockPos = transform.localPosition;
        currentLive = blockLives;
    }

    private void OnValidate()
    {
        if (delayShake > time) delayShake = time;
    }

    void OnCollisionEnter2D(Collision2D hit) {
        if(hit.gameObject.tag == "Ball"){
            OnBlockGetHit();
            if(blockLives <= 0){
               OnBlockGetDestroy();
            }
        }
	}

    void OnBlockGetHit(){
        blockLives -= 1;
        ChangeSprite();
        StopAllCoroutines();
        StartCoroutine(shakeObject());
    }

    void ChangeSprite(){
        if(blockLives < currentLive){
            spriteBlock.sprite = spriteDamaged;
        }
    }

    void OnBlockGetDestroy(){
        var pm = particle.main;
        BricksManager.Instance.RemainingBricks.Remove(this.gameObject);
        GameManager.Instance.OnBrickEmpty();

        spriteBlock.enabled = false;
        particle.transform.position = transform.position;
        pm.startColor = spriteBlock.color;
        pm.loop = false;
        particle.Play();
        Destroy(particle.gameObject, pm.duration);
    }

    IEnumerator shakeObject(){
        timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
    
            randomPos = blockPos + (Random.insideUnitSphere * distance);
            transform.localPosition = randomPos;

            if (delayShake > 0f)
            {
                yield return new WaitForSeconds(delayShake);
            }
            else
            {
                yield return null;
            }
        }
 
       transform.localPosition = blockPos;
   }
    
}
