using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    //Inspector variable
    public AudioSource buttonSound;

    public void StartButtonPressed()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void ButtonSound()
    {
        buttonSound.Play();
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    public void MenuButtonPressed()
    {
        SceneManager.LoadScene("Menu");
        PermanentUI.perm.Reset();
    }
}
