using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEvent : MonoBehaviour
{
    //brick param
    public int blockLives = 1;
    public int blockScore = 10;
    public Sprite spriteDamaged;
    private int currentLive;

    //shake
    private float time = 0.2f;
    private float distance = 0.1f;
    private float delayShake = 0f;

    private Vector3 blockPos;
    private float timer;
    private Vector3 randomPos;
    private SpriteRenderer spriteBlock;
    private ParticleSystem particle;
    private BoxCollider2D colBox;
    
    void Awake()
    {
        colBox = GetComponent<BoxCollider2D>();
        spriteBlock = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }

    void Start()
    {
        blockPos = transform.localPosition;
        colBox.enabled = true;
        currentLive = blockLives;
    }

    private void OnValidate()
    {
        if (delayShake > time) delayShake = time;
    }

    void OnCollisionEnter2D(Collision2D hit) {
        if(hit.gameObject.tag == "Ball"){
            CheckBrickGetHit();
        }
	}

    public void CheckBrickGetHit(){
        OnBlockGetHit();
        if(blockLives <= 0){
            OnBlockGetDestroy();
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
        OnManagerCheckEvent();
        DestroyBrick();
    }

    void OnManagerCheckEvent(){
        AudioManager.instance.Play("Break");
        BricksManager.Instance.RemainingBricks.Remove(this.gameObject);
        GameManager.Instance.AddScore(blockScore);
        GameManager.Instance.OnBrickDestroy();
    }

    void DestroyBrick(){
        spriteBlock.enabled = false;
        colBox.enabled = false;

        particle.transform.position = transform.position;
        var pm = particle.main;
        pm.startColor = spriteBlock.color;
        pm.loop = false;
        particle.Play();
        Destroy(particle.gameObject, pm.duration);
        InstantiateCollectible();
    }
    
    void InstantiateCollectible(){
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float deBuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= BricksManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            Collectibles newBuff = this.SpawnCollectable(true);
        }

        if (deBuffSpawnChance <= BricksManager.Instance.DebuffChance && !alreadySpawned)
        {
            Collectibles newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectibles SpawnCollectable(bool isBuff)
    {
        List<Collectibles> collection;

        if (isBuff)
        {
            collection = BricksManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = BricksManager.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectibles prefab = collection[buffIndex];
        Collectibles newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectibles;

        return newCollectable;
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
