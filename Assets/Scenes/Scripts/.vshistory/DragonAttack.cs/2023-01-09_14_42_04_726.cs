using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private bool isAttackPressed;
    private bool isAttacking;

    public float timeBetweenShots = 2f;
    private float timeStamp;
    private DragonMovement dragonMovement;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;
    [SerializeField] private float attackDelay = 10f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }
    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
        {
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isAttacking = true;
            FireAttack();
        }
    }

    private void FireAttack()
    {
        if (Time.time >= timeStamp && (Input.GetKeyDown(KeyCode.E)))
        {
            //Instantiate(bullet, transform.position, transform.rotation);

            timeStamp = Time.time + timeBetweenShots;

            if (isAttackPressed)
            {
                isAttackPressed = false;

                if (!isAttacking)
                {
                    isAttacking = true;
                    if (dragonMovement.canAttack())
                    {
                        anim.SetBool("isAttack", true);
                    }
                    else
                    {
                        anim.SetBool("isAttack", false);
                    }
                    //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
                    Invoke("AttackComplete", attackDelay);
                    Debug.Log("delay attack works");

                    fireBalls[FindFireBall()].transform.position = firePoint.position;
                    fireBalls[FindFireBall()].GetComponent<BulletController>().SetDirection(Mathf.Sign(transform.localScale.x));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("Explode");
        Debug.Log("explode");
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        float localScaleX = transform.localScale.x;

        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        Debug.Log("setdirection works");
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
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

    private void Deactive()
    {
        gameObject.SetActive(false);
    }
}
