using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float Speed =4;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 retning = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            retning.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            retning.x = -1;
        }
        //normalize sætter længden på vectoren til 1
        retning = retning.normalized;
        //transform.position += retning *Time.deltaTime * Speed;
        retning *= Speed;
        retning.y = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("Jump");
            retning.y = 5;
            grounded = false;
        }
        rb.velocity = retning;

        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if(collision.contacts[0].normal.y > 0.9f)
        {
            grounded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.9f)
        {
            grounded = false;
        }
    }
}
