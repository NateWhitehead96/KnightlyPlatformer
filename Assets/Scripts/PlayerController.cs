/*Playercontroller
 * Nathan Whitehead
 * 101242269
 * 12/7/20
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

    public AudioSource coinCollect;
    public AudioSource enemyHurt;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        lives = 3;
    }

    // Update is called once per frame
    void FixedUpdate() // Will display your health in the UI and destroy icons based on missing health
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
        // Attack button, starts a coroutine for the attack animation and damage mechanic
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        animator.SetInteger("AnimState", 4); // attacking
        attacking = true;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyMask); // create a small circle and if enemy is inside deal damage
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyBehaviour>().health -= 1;
            enemyHurt.Play();
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
    { // our ground checks. no falling through floors here
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
            coinCollect.Play();
        }
    }


    IEnumerator Hurt()
    { // a coroutine that resets the player if they fall into spikes
        animator.SetInteger("AnimState", 3);
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(-2.45f, -0.6f);
        animator.SetBool("isHurt", false);
    }

    private void OnDrawGizmosSelected()
    { // for dev purposes to see our attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
