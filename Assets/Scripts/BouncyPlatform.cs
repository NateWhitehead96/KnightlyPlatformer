/*BouncyPlatform
 * Nathan Whitehead
 * 101242269
 * 12/2/20
 * The bouncy platform behaviour
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float force = 8f;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force * Time.deltaTime);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2 * Time.deltaTime); // push the player a little too for ease of movement
        }
    }
}
