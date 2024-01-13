using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    public static int life =3;

    public AudioSource boostPlayer;

    public AudioSource jumpPlayer;
    public AudioSource avPlayer;
    public AudioSource breakPlayer;

    public Animator anim;
    public SpriteRenderer rend;

    Rigidbody2D rb;
    public float MaxSpeed = 5;
    float Speed = 500;
    bool grounded;
    bool onWall;
    bool onBooster;
    bool DoubleHeight;
    bool boostActivated;
    bool HasShotFromBooster;

    Transform booster;

    Vector2 wallNormal;
    public float jumpForce = 500;
    public float BounceForce = 650;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    public void PlayAv()
    {
        avPlayer.Play();
    }
    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Grounded", grounded);
        Debug.Log(Mathf.Abs(rb.velocity.x) > 0.1f);
        anim.SetBool("Walking", Mathf.Abs(rb.velocity.x) > 0.3f);
        if (rb.velocity.x < 0) 
            rend.flipX = true;
        else
            rend.flipX = false;

        if (life <= 0)
        {
            GoToDeathScreen();
        }

        Vector3 retning = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            retning.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            retning.x = -1;
        }
        else if( grounded)
        {

            retning.x = -Mathf.Clamp( rb.velocity.x,-1,1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            boostActivated = true;
            rb.AddForce(retning * jumpForce);
            StartCoroutine(SpeedBoost(0.3f));
        }

        retning *= Speed;
        if(onBooster && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.velocity = Vector2.zero;
            Vector2 JumpDir = booster.up;
            onBooster = false;
            HasShotFromBooster = true;
            rb.AddForce(JumpDir *jumpForce *booster.GetComponent<BoosterMultiplier>().Multiplier);
            boostActivated = true;
            boostPlayer.Play();
            StartCoroutine(SpeedBoost(0.5f, true));
        }
        else if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.W)) && (grounded || onWall))
        {
            jumpPlayer.Play();
            Vector2 JumpDir = Vector2.zero;
            JumpDir.y = 1;

            if (onWall && !grounded)
            {
                JumpDir.x = wallNormal.x;
                onWall = false;
            }

            grounded = false;
            if (DoubleHeight)
            {
                rb.AddForce(JumpDir.normalized * BounceForce);
                DoubleHeight = false;
            }
            else
            {
                rb.AddForce(JumpDir.normalized * jumpForce);
            }
               
        }

        if(!grounded && Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(Vector2.down * jumpForce*2.5f);
            DoubleHeight = true;
        }

        rb.AddForce(retning * Time.deltaTime);

        if (!boostActivated)
        {
            Vector2 currentVelocity = rb.velocity;
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -MaxSpeed, MaxSpeed);
            rb.velocity = currentVelocity;
        }
        
        
    }
    IEnumerator SpeedBoost(float time, bool resetDest = false)
    {
        yield return new WaitForSeconds(time);
        boostActivated = false;
        if (resetDest) HasShotFromBooster = false;
    }

    IEnumerator resetDouble()
    {
        yield return new WaitForSeconds(0.2f);
        DoubleHeight = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Booster")
        {
            onBooster = true;
            booster = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onBooster = false;
    }

    void GoToDeathScreen()
    {
        life = 3;
        SceneManager.LoadScene("Death");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GoToDeathScreen();
        }
        
        if(collision.gameObject.tag == "Destructable" && HasShotFromBooster)
        {
            breakPlayer.Play();
            Destroy(collision.gameObject);
        }
        
        if (DoubleHeight)
        {
            StartCoroutine(resetDouble());
        }
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.9f)
            {
                grounded = true;
            }
            if (Mathf.Abs(collision.contacts[i].normal.x) > 0.9f && !grounded)
            {
                onWall = true;
                wallNormal = collision.contacts[i].normal;
            }
        }

        HasShotFromBooster = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.9f)
            {
                grounded = true;
            }
            if (Mathf.Abs(collision.contacts[i].normal.x) > 0.9f && !grounded)
            {
                onWall = true;
                wallNormal = collision.contacts[i].normal;
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
        onWall = false;
    }
}
