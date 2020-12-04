using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody;
    public bool isMoving = true;
    public bool isAttacking = false;
    public int health;
    public int patrolLeftDistance;
    public int patrolRightdistance;
    public int direction;
    public GameObject droppedLoot;

    public Transform attackPos;
    public LayerMask playerMask;
    public float attackRange;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        animator.SetInteger("EnemyState", 2);
        isMoving = false;
        isAttacking = false;
        yield return new WaitForSeconds(2f);
        Instantiate(droppedLoot, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Move()
    {
        if (isMoving && !isAttacking)
        {
            transform.position = new Vector3(transform.position.x + direction * Time.deltaTime, transform.position.y);

            if (transform.position.x >= patrolRightdistance)
            {
                direction = -1;
                transform.localScale = new Vector3(0.03f, 0.03f, 1f);
            }
            if (transform.position.x <= patrolLeftDistance)
            {
                direction = 1;
                transform.localScale = new Vector3(-0.03f, 0.03f, 1f);
            }
            animator.SetInteger("EnemyState", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        animator.SetInteger("EnemyState", 1); // set enemy to attack mode
        isMoving = false; // stop the movement
        isAttacking = true;
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<PlayerController>().lives -= 1;
            player[i].GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * Time.deltaTime);
        }
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        isMoving = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if(isAttacking)
    //        {
    //            Debug.Log("Enemy is attacking!");
    //            collision.gameObject.GetComponent<PlayerController>().lives -= 1;
    //            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * 5 * Time.deltaTime);
    //        }
    //    }
    //}

}
