/*Playercontroller
 * Nathan Whitehead
 * 101242269
 * 11/20/20
 * The script for controlling score keeping
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public int score;
    public Text scoreText;

    private void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
