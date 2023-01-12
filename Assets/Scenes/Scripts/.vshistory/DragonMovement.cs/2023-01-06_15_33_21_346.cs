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
    //public Transform overheadCheckCollider;
    public Collider2D standingCollider, crouchingCollider;
    const float overheadCheckRadius = 0.2f;
    private bool crouchPressed = false;
    float crouchSpeedModifier = 0.5f;
    [SerializeField] float speed = 2f;

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

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
        {
            crouchPressed = true;
            moveSpeed = 0f;

        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            crouchPressed = false;
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
        if (IsGrounded())
        {
            standingCollider.enabled = !crouchFlag;
        }

        float xVal = dirX * speed * 100 * Time.fixedDeltaTime;

        if (crouchFlag)
        {

        }

        anim.SetBool("crouch", crouchFlag);
        standingCollider.enabled = !crouchFlag;
        crouchingCollider.enabled = crouchFlag;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
