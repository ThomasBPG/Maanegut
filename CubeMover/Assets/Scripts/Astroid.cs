using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    public float Speed = 5;
    Rigidbody2D rb;
    bool dangerous = true;
    public GameObject bum;
    public GameObject luftSten, LandSten;

    
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
            
            if (dangerous)
                Player.life--;
            Player.instance.PlayAv();
            Destroy(gameObject);
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        dangerous = false;
        
        bum.SetActive(true);
        luftSten.SetActive(false);
        LandSten.SetActive(true);
    }
}
