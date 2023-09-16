using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    //Inspector variables
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private Collider2D coll;
    [SerializeField] private LayerMask ground;

    private bool facingRight = true;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //Transition from Jump to Fall
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //Transition from Fall to Idle
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

    //Enemy AI movement
    private void Move()
    {
        if (facingRight)
        {
            if (transform.position.x < rightCap)
            {
                //Make sure sprite is facing the right location, and if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //Test to see if I am on the ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingRight = false;
            }
        }

        else
        {
            if (transform.position.x > leftCap)
            {
                //Make sure sprite is facing the right location, and if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //Test to see if I am on the ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingRight = true;
            }
        }
    }
}
