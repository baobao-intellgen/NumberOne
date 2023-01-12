using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    //[SerializeField] private AudioSource jumpSoundEffect;

    public LayerMask groundLayer;
    public Transform overheadCheckCollider;
    public Collider2D standingCollider, crouchingCollider;
    const float overheadCheckRadius = 0.2f;
    private bool crouchPressed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        /*if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
        {

        }*/

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                crouchPressed = true;
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                crouchPressed = false;
            }

            else
            {
                return;
            }
        }

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        Move(dirX, crouchPressed);
    }

    private void UpdateAnimationState()
    {


        if (dirX > 0f)
        {
            anim.SetBool("run", true);
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            anim.SetBool("run", true);
            sprite.flipX = true;
        }
        else
        {
            anim.SetBool("run", false);
        }

        if (rb.velocity.y > .1f)
        {
            anim.SetBool("jump", true);
            Debug.Log("jump"); 
        }
        else if (IsGrounded())
        {
            anim.SetBool("jump", false);
            Debug.Log("isGrounded");
        }
    }

    private void Move(float dir, bool crouchFlag)
    {
        if (!crouchFlag)
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
                crouchFlag = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
