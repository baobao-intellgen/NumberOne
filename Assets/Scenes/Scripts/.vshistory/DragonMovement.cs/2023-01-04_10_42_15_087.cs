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

    bool jump;
    bool crouch;
    bool isRunning;

    //[SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        #region Run - Jump

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        jump = false;
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            anim.SetBool("jump", true);
            anim.SetBool("run", false);
            //jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        #endregion

        #region Crouch

        if (Input.GetKeyDown(KeyCode.S) && IsGrounded())
        {
            crouch = true;
            anim.SetBool("crouch", true);
        }

        else if (Input.GetKeyUp(KeyCode.S) && IsGrounded())
        {
            crouch = false;
            anim.SetBool("crouch", false);
        }

        #endregion

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            sprite.flipX = false;
            jump = false;
            anim.SetBool("run", true);
            anim.SetBool("jump", false);
        }
        else if (dirX < 0f)
        {
            sprite.flipX = true;
            anim.SetBool("run", true);
            anim.SetBool("jump", false);
        }
        else
        {
            anim.SetBool("run", false);
        }

        if (rb.velocity.y > .1f)
        {
            anim.SetBool("jump", true);
            anim.SetBool("run", false);
        }
        
        else if (rb.velocity.y < .1f)
        {
            anim.SetBool("jump", false);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
