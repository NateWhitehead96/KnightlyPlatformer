/*FloatingPlatform
 * Nathan Whitehead
 * 101242269
 * 12/2/20
 * The floating platforms behaviour
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    private int direction = 1;
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x + direction * Time.deltaTime, transform.position.y);

        if(transform.position.x >= 46.5)
        {
            direction = -1;
        }
        if (transform.position.x <= 36.5)
        {
            direction = 1;
        }
    }

}
