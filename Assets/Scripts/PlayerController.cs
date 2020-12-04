/*Playercontroller
 * Nathan Whitehead
 * 101242269
 * 12/4/20
 * The player's controlling script for movement, animations, and anything else player related
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private Animator animator;
    public int lives = 3;

    public Transform attackPos;
    public LayerMask enemyMask;
    public float attackRange;

    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool grounded;
    public bool jumping = false;
    public bool attacking = false;
    public Image lives1;
    public Image lives2;
    public Image lives3;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lives = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if(lives == 2)
        {
            Destroy(lives3);
        }
        if(lives == 1)
        {
            Destroy(lives2);
        }
        if(lives <= 0)
        {
            Destroy(lives1);
            SceneManager.LoadScene("Lose");
        }
    }

    private void Move()
    {
        if (grounded && !jumping)
        {
            if (!attacking)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    rigidbody.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    animator.SetInteger("AnimState", 2); // run
                    transform.localScale = new Vector3(0.03f, 0.03f, 1);
                    //sprite.flipX = false;
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    rigidbody.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    animator.SetInteger("AnimState", 2); // run
                    transform.localScale = new Vector3(-0.03f, .03f, 1);
                    //sprite.flipX = true;
                }
                else
                    animator.SetInteger("AnimState", 0); // idle
            }
            rigidbody.velocity *= 0.9f;
        }
    }

    public void AButton()
    {
        // Attack button, but for now will switch to win scene
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        animator.SetInteger("AnimState", 4);
        attacking = true;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyBehaviour>().health -= 1;
            //enemies[i].GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2 * Time.deltaTime);
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    public void BButton()
    {
        // Jump button
        if (grounded && !attacking)
        {
            rigidbody.AddForce(Vector2.up * verticalForce * Time.deltaTime);
            animator.SetInteger("AnimState", 1); // jump
            jumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform") || collision.gameObject.CompareTag("BreakingPlatform")) // if colliding with ground we're grounded and not jumping
        {
            grounded = true;
            jumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Hazard"))
        {
            lives -= 1;
            animator.SetBool("isHurt", true);
            //transform.position = new Vector3(-2.45f, -0.6f);
            StartCoroutine(Hurt());
        }

        if (other.gameObject.CompareTag("ExitDoor"))
        {
            SceneManager.LoadScene("Win");
        }

        if (other.gameObject.CompareTag("Coin")) // if colliding with coins add to score
        {
            Destroy(other.gameObject);
            ScoreManager.score += 1;
        }
    }


    IEnumerator Hurt()
    {
        animator.SetInteger("AnimState", 3);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(-2.45f, -0.6f);
        animator.SetBool("isHurt", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
