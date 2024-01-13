using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    public float Speed = 5;
    Rigidbody2D rb;
    bool dangerous = true;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), -1, 0);
        rb.velocity = direction * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag  == "Player")
        {
            UIhandler.collectedStones++;
            Destroy(gameObject);
            if (dangerous)
                Player.life--;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        dangerous = false;
    }
}
