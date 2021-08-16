using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector2 direction;
    private Vector2 newPos;
    private Rigidbody2D rb;
    private CircleCollider2D ccol;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ccol = GetComponent<CircleCollider2D>();
        direction = Vector2.down.normalized;
        transform.Rotate(direction);    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = direction.normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        SetDirectionOnCollison();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.name == "DeathWall" 
        || col.gameObject.tag == "Arrow" || col.gameObject.tag == "Ball"){
            AudioManager.instance.Play("MobDie");
            Destroy(this.gameObject);
        }

        if(col.gameObject.tag == "Player"){
             GameManager.Instance.AddScore(50);
        }

        if(col.gameObject.tag == "Ball"){
             GameManager.Instance.AddScore(100);
        }
    }

    void SetDirectionOnCollison(){
        Vector2 randomDir;
        int randNum = Random.Range(0, 4);

        switch (randNum)
        {
            case 0:
                randomDir = Vector2.down;
            break;
            case 1:
                randomDir = Vector2.up;
            break;
            case 2:
                randomDir = Vector2.left;
            break;
            case 3:
                randomDir = Vector2.right;
            break;
            default:
                randomDir = Vector2.down;
            break;
        }
        direction = randomDir;
    }
    

}
