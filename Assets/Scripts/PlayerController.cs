/*Playercontroller
 * Nathan Whitehead
 * 101242269
 * 11/20/20
 * The player's controlling script for movement, animations, and anything else player related
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite;

    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool grounded;
    public bool jumping = false;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (grounded)
        {
            if (!attacking)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    rigidbody.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    //sprite.flipX = false;
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    rigidbody.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    //sprite.flipX = true;
                }
            }
            rigidbody.velocity *= 0.9f;
        }
    }

    public void AButton()
    {
        // Attack button, but for now will switch to win scene
        SceneManager.LoadScene("Win");
    }

    public void BButton()
    {
        // Jump button
        rigidbody.AddForce(Vector2.up * verticalForce * Time.deltaTime);
        jumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // if colliding with ground we're grounded and not jumping
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
            // fell on spikes right now will 1 shot you
            SceneManager.LoadScene("Lose");
        }


        if (other.gameObject.CompareTag("Coin")) // if colliding with coins add to score
        {
            Destroy(other.gameObject);
            ScoreManager.score += 1;
        }
    }
}
