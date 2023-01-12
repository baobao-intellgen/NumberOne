using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovenments : MonoBehaviour
{
    /*public int maxHealth = 100;
    public int currentHealth;*/
    public CharacterStatus healthBar;
    public float timeBetweenShots = 2f;
    private float timeStamp;
    

    /* --------------------------------------*/
    private Collider2D coll;
    private float horizontal = 0;
    private float vertical = 0;
    private Animator anim;
    private SpriteRenderer spriRender;

    /* --------------------------------------*/
    private string currentState;

    private bool isJumping;
    private bool isFalling;
    private bool isRunning;
    private bool isGround;
    private bool grounded;
    private bool currentAnimation;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool isDizzy;
    private bool isDeath;
    private bool isCrouched;
    private bool isCrouching;
    private bool isCrouchAttack;
    private bool isHurt;

    [SerializeField] private float attackDelay = 10f;

    /* --------------------------------------*/

    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public float dirX, dirY;
    private float wallJumpCooldown;

    //private enum MovementState { idle, running, falling, jumping }

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    public bool facingLeft;

    /* --------------------------------------*/
    void Start()
    {
        /*currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);*/
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriRender = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        if (facingLeft == true)
        {
            facingLeft = false;
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontal * dirX, rb.velocity.y);

        anim.SetBool("run", horizontal != 0);
        anim.SetBool("grounded", isGround);


        if (wallJumpCooldown > 0.2f)
        {
            rb.velocity = new Vector2(horizontal * dirX, rb.velocity.y);

            /*if (*//*onWall() && *//*!isGrounded())
            {
                //body.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }*/

            if (Input.GetKey(KeyCode.Space))
                Jump();
            Debug.Log("jump()");
        }
        else
            wallJumpCooldown += Time.deltaTime;

        // Attack
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAttackPressed = true;
            isAttacking = false;
        }
         

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeMedicine(15);
        }*/

        /*  if (Time.time >= timeStamp && (Input.GetKeyDown(KeyCode.E)))
          {

              timeStamp = Time.time + timeBetweenShots;

              if (isAttackPressed)
              {
                  isAttackPressed = false;

                  if (!isAttacking)
                  {
                      isAttacking = true;
                      if (isGround)
                      {

                      }
                      else
                      {
                          //ChangeAnimationState(PLAYER_AIR_ATTACK);
                      }
                      //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
                      Invoke("AttackComplete", attackDelay);
                      Debug.Log("delay attack works");

                      fireBalls[FindFireBall()].transform.position = firePoint.position;
                      fireBalls[FindFireBall()].GetComponent<BulletController>().SetDirection(Mathf.Sign(transform.localScale.x));
                  }
              }
          }

          // Crouch

          if (Input.GetKeyDown(KeyCode.S))
          {
              isCrouching = true;
              //ChangeAnimationState(PLAYER_CROUCH);
              Debug.Log("Crouch Attack");
          }

          if (Input.GetKeyDown(KeyCode.S))
          {
              if (isCrouching)
              {
                  //isCrouching = false;
                  if (isGround)
                  {


                      //ChangeAnimationState(PLAYER_CROUCH);
                      Debug.Log("is crouched");
                  }

                  //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
              }

          }
          */

        //UpdateAnimationState();

        /*if (GetComponent<CharacterStatus>().currentHealth < 0)
        {
            //ChangeAnimationState(PLAYER_HURT);
        }
        else
        {
            if (!isDeath)
            {
                //ChangeAnimationState(PLAYER_DEATH);
                gameObject.SetActive(false);
                isDeath = true;
            }
        }*/


    }

    /*private void UpdateAnimationState()
    {
        MovementState state;

        if (horizontal > 0f)
        {
            state = MovementState.running;
            spriRender.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.running;
            spriRender.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.falling;
        }

        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }*/

    /*private void IsGrounded()
    {
        //isGround = false;
        isGround = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f, groundLayer);
        if (colliders.Length > 0)
        {
            isGround = true;
        }
        
        if (isGround)
        {
            Debug.Log("Ground check work");
        }
    }*/


    private void FixedUpdate()
    {
        //Ground Check
        isGround = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);

        /*void FallGroundAfterJump()
        {
            if (isGround && !isJumping && horizontal == 0f && !isCrouching)
            {
                
            }
        }*/

        

        //Flip X
        if (facingLeft == true && horizontal > 0f)
        {
            //spriRender.flipX = false;
            Flip();
        }
        else if (facingLeft != true && horizontal < 0f)
        {
            //spriRender.flipX = true;
            Flip();
        }

        /*if (isCrouched)
        {
             isCrouched = false;
             if (!isCrouching)
             {
                 isCrouching = true;
                 if (isGround)
                 {
                     ChangeAnimationState(PLAYER_CROUCH);
                     Debug.Log("is crouched");
                 }
                 //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
                 Invoke("CrouchComplete", attackDelay);
                
             }
        }*/


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tag")
        {
            grounded = true;
        }
    }

    public bool CanAttackOnGround()
    {
        return horizontal == 0 && isGround && grounded;
    }

    void AttackComplete()
    {
        isAttacking = false;
        Debug.Log("AttackComplete() works");
    }

    void CrouchComplete()
    {
        isCrouching = false;
    }


    /*public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void TakeMedicine(int medicine)
    {
        currentHealth += medicine;
        healthBar.SetHealth(currentHealth);
    }*/

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 theScale = new Vector2();
        theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
    }

    void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, dirY);
            anim.SetTrigger("jump");
            Debug.Log("set run");
        }
        else if (!isGrounded())
        {
            if (horizontal == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                //transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            }
            wallJumpCooldown = 0;
        }
        
    }

    private int FindFireBall()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }

    //public GameObject bullet;
}
