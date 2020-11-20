/*SceneManger
 * Nathan Whitehead
 * 101242269
 * 11/20/20
 * The script for all UI buttons for scene switching
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public AudioSource buttonSound;
    public void Back()
    {
        buttonSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void HowToPlay()
    {
        buttonSound.Play();
        SceneManager.LoadScene("HowToPlay");
    }

    public void Play()
    {
        buttonSound.Play();
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        buttonSound.Play();
        Application.Quit();
    }
}
