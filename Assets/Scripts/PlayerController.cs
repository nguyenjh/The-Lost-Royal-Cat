using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Inspector variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 40f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource itemCollect;
    

    //Finite state machine
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;

    private void Start()
    {
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
    }

    private void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); //Sets animation based on Enumerator state
    }

    //When player collects fish or powerup
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            itemCollect.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.fish += 1;
            PermanentUI.perm.fishText.text = PermanentUI.perm.fish.ToString();
        }
        if (collision.tag == "Powerup")
        {
            itemCollect.Play();
            Destroy(collision.gameObject);
            jumpForce = 40f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Enemy variable
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HandleHealth(); //Deals with health, updating UI, and will reset level if health is <= 0
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right, therefore I should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left, therefore I should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    //Deals with player health stats and reseting the scene when player has health amount <= 0
    private void HandleHealth()
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            PermanentUI.perm.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //Player movement
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //Moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    //Player jump
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    //Player animations
    private void AnimationState()
    {


        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }

        else
        {
            state = State.idle;
        }
    }

    //Player footstep sounds
    private void Footstep()
    {
        if (coll.IsTouchingLayers(ground))
        {
            footstep.Play();
        }
    }

    //Jump-boost powerup reseting its power after 5 seconds
    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 30;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
