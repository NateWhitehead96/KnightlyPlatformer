/*BreakingPlatform
 * Nathan Whitehead
 * 101242269
 * 12/2/20
 * The breaking platforms behaviour
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public GameObject self;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Break());
        }
    }

    IEnumerator Break()
    {
        // maybe play rumble animation
        yield return new WaitForSeconds(1f);
        Destroy(self);
    }
}
